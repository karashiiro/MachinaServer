using MachinaWrapper.Common;
using Sapphire.Common.ActorControl;
using Sapphire.Common.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NameSizePair = System.Collections.Generic.KeyValuePair<string, int>;

namespace MachinaWrapper.Parsing
{
    public class Parser
    {
        private class OpcodeInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("opcode")]
            public ushort Opcode { get; set; }
        }

        private class OpcodeSublists
        {
            public OpcodeInfo[] ServerZoneIpcType { get; set; }
            public OpcodeInfo[] ClientZoneIpcType { get; set; }
            public OpcodeInfo[] ServerLobbyIpcType { get; set; }
            public OpcodeInfo[] ClientLobbyIpcType { get; set; }
            public OpcodeInfo[] ServerChatIpcType { get; set; }
            public OpcodeInfo[] ClientChatIpcType { get; set; }
        }

        private class OpcodeList
        {
            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("lists")]
            public OpcodeSublists Lists { get; set; }
        }

        public int Capacity;
        public List<NameSizePair> MessageSizes;
        public ParserMode Mode;
        public Region Region;

        public uint Port;

        private OpcodeList[] OpcodeLists;
        private OpcodeList ActiveOpcodeList;

        private readonly uint Modulator = (uint)new Random().Next(int.MinValue, int.MaxValue);

        private static readonly HttpClient http = new HttpClient(); // For sending data to Node.js

        public Parser(Region region, uint port)
        {
            Capacity = 50; // Initialize the StringBuilder with 50 characters.
            MessageSizes = new List<NameSizePair>();
            Mode = ParserMode.RAMHeavy;
            Region = region;
            Port = port;
        }

        public Parser(Region region, ParserMode mode, uint port) : this(region, port)
        {
            Mode = mode;
        }

        public Parser(Region region, ParserMode mode, uint port, int capacity) : this(region, mode, port)
        {
            Capacity = capacity;
        }

        public Parser(Region region, ParserMode mode, uint port, List<NameSizePair> capacities) : this(region, mode, port)
        {
            MessageSizes = capacities;
        }

        public async Task Initialize()
        {
            string rawOpcodes;
            try
            {
                rawOpcodes = await http.GetStringAsync(
                    new Uri("https://cdn.jsdelivr.net/gh/karashiiro/FFXIVOpcodes@latest/opcodes.min.json"));
            }
            catch (Exception e)
            {
                if (e is HttpRequestException || e is WebException)
                {
                    await Console.Error.WriteLineAsync(e.Message);
                    rawOpcodes = await http.GetStringAsync(
                        new Uri("https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/opcodes.min.json"));
                }
                else throw;
            }
            OpcodeLists = JsonConvert.DeserializeObject<OpcodeList[]>(rawOpcodes);
            ActiveOpcodeList = OpcodeLists.FirstOrDefault(l => l.Region == Region.ToString());
        }

        /// <summary>
        /// Structures the data into a JSON string, and does some preliminary byte parsing.
        /// </summary>
        public void Parse(Packet meta)
        {
            if (meta.Data.Length == 0) // See #41, this shouldn't happen but it somehow has in that case, may as well add this
            {
                Trace.WriteLine("Received packet with no IPC header or body, skipping...");
                return;
            }

            // Checking endianness
            if (!BitConverter.IsLittleEndian)
            {
                meta.Data = meta.Data.Reverse().ToArray();
            }

            // Get packet size.
            meta.PacketSize = BitConverter.ToUInt32(meta.Data, (int)Offsets.PacketSize);

            // Actor ID obfuscation for some measure of privacy, discouraging stalking and the like.
            // Ideally this data could just be wiped out, but that would make IPC data useless for minimap features.
            var sourceActorID = (int)Offsets.SourceActor + 4 < meta.PacketSize ? BitConverter.GetBytes(BitConverter.ToUInt32(meta.Data, (int)Offsets.SourceActor) ^ Modulator) : new byte[4];
            var targetActorID = (int)Offsets.TargetActor + 4 < meta.PacketSize ? BitConverter.GetBytes(BitConverter.ToUInt32(meta.Data, (int)Offsets.TargetActor) ^ Modulator) : new byte[4];
            // Copy obfuscated data in
            for (var i = 0; i < 4; i++)
            {
                meta.Data[(int)Offsets.SourceActor + i] = sourceActorID[i];
                meta.Data[(int)Offsets.TargetActor + i] = targetActorID[i];
            }

            // Get IPC data, if applicable.
            meta.SegmentType = (int)Offsets.SegmentType + 2 < meta.PacketSize ? BitConverter.ToUInt16(meta.Data, (int)Offsets.SegmentType) : new ushort();
            var ipcData = new IpcPacket(meta);
            if (meta.SegmentType == 3) // IPC segment type
            {
                ProcessIPCData(ref ipcData);
            }

            ipcData.Type ??= "unknown"; // Check if the property name exists
            Util.JSify(ref ipcData.Type);

            switch (Mode)
            {
                case ParserMode.RAMHeavy:
                    OutputRAMHeavy(ipcData);
                    break;
                case ParserMode.CPUHeavy:
                    OutputCPUHeavy(ipcData);
                    break;
                case ParserMode.PacketSpecific:
                    OutputPacketSpecific(ipcData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Structures the data into a JSON string, prints it to the console. This uses a StringBuilder sized to the largest packet recieved.
        /// This can use quite a bit of RAM and can crash low-memory systems if many packets are being parsed at once. Modern computers can run this happily.
        /// </summary>
        private void OutputRAMHeavy(IpcPacket ipcData)
        {
            // This is just a slight modification of the CPU-heavy logger.
            var sb = OutputCPUHeavy(ipcData);

            // Update the size so that future StringBuilders don't need to repeatedly resize themselves.
            // This is likely to be the main cause of memory usage increases.
            if (sb.Capacity > Capacity)
            {
                Capacity = sb.Capacity;
            }
        }

        /// <summary>
        /// Structures the data into a JSON string, prints it to the console. This uses a StringBuilder sized to the largest packet recieved of each packet type.
        /// This uses less memory than the RAM-heavy method, but it uses more CPU time in exchange. This is probably the best option for low-spec systems.
        /// </summary>
        private void OutputPacketSpecific(IpcPacket ipcData)
        {
            // Fetch the StringBuilder capacity.
            NameSizePair ipcMessageSize;
            if (MessageSizes.Exists((nsp) => nsp.Key == ipcData.Type))
            {
                ipcMessageSize = MessageSizes.Find((nsp) => nsp.Key == ipcData.Type);
            }
            else
            {
                ipcMessageSize = new NameSizePair(ipcData.Type, Capacity);
                MessageSizes.Add(ipcMessageSize);
            }

            // This is a modification of the CPU-heavy logger.
            var sb = OutputCPUHeavy(ipcData);

            // Update the size so that future StringBuilders don't need to repeatedly resize themselves.
            if (ipcData.Type != "unknown" && sb.Capacity > ipcMessageSize.Value)
            {
                MessageSizes.Remove(ipcMessageSize);
                ipcMessageSize = new NameSizePair(ipcData.Type, sb.Capacity);
                MessageSizes.Add(ipcMessageSize);
            }
        }

        /// <summary>
        /// Structures the data into a JSON string, prints it to the console. This uses a StringBuilder that sizes dynamically to each packet.
        /// This uses quite a bit of runtime resizing the StringBuilder repeatedly, and will cause a noticable slowdown in-game if many packets are being parsed at once.
        /// </summary>
        private StringBuilder OutputCPUHeavy(IpcPacket ipcData, int? capacity = null)
        {
            // The JSON consists of potentially useful header information and the IPC data if it exists.
            // Heavy data processing is done on the Node side, since it's easier to test packet structures like that.
            var JSON = new StringBuilder(capacity ?? Capacity);
            JSON.Append("{\"type\":\"").Append(ipcData.Type).Append("\",")
                .Append("\"opcode\":").Append(ipcData.Opcode).Append(",")
                .Append("\"region\":\"").Append(Region).Append("\",")
                .Append("\"connection\":\"").Append(ipcData.Metadata.ConnectionRoute).Append("\",")
                .Append("\"operation\":\"").Append(ipcData.Metadata.ConnectionType).Append("\",")
                .Append("\"epoch\":").Append(ipcData.Metadata.Epoch).Append(",")
                .Append("\"packetSize\":").Append(ipcData.Metadata.PacketSize).Append(",")
                .Append("\"segmentType\":").Append(ipcData.Metadata.SegmentType).Append(",");
            if (ipcData.Metadata.SegmentType == 3)
            {
                // IPC header info
                JSON.Append("\"sourceActorSessionID\":").Append(BitConverter.ToUInt32(ipcData.Metadata.Data, (int)Offsets.SourceActor)).Append(",")
                    .Append("\"targetActorSessionID\":").Append(BitConverter.ToUInt32(ipcData.Metadata.Data, (int)Offsets.TargetActor)).Append(",")
                    .Append("\"serverID\":").Append(ipcData.ServerId).Append(",")
                    .Append("\"timestamp\":").Append(ipcData.Timestamp).Append(",");
                
                // Trim useless information
                ipcData.Metadata.Data = ipcData.Metadata.Data.Skip((int)Offsets.IpcData).ToArray();

                // If the IPC type isn't unknown we might have category data sitting around
                if (ipcData.ActorControlCategory != null)
                {
                    JSON.Append("\"superType\":\"actorControl\",")
                        .Append("\"subType\":\"").Append(ipcData.ActorControlCategory).Append("\",");
                }
                else if (ipcData.ClientTriggerCategory != null)
                {
                    JSON.Append("\"superType\":\"clientTrigger\",")
                        .Append("\"subType\":\"").Append(ipcData.ClientTriggerCategory).Append("\",");
                }
            }
            JSON.Append("\"data\":[").Append(string.Join(",", ipcData.Metadata.Data)).Append("]}");

            var message = new StringContent(JSON.ToString(), Encoding.UTF8, "application/json");

#if DEBUG
            Console.WriteLine(JSON.ToString());
#else
            try
            {
                http.PostAsync("http://localhost:" + Port, message);
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine(e.Message);
            }
#endif

            return JSON;
        }

        /// <summary>
        /// Converts opcodes into readable strings.
        /// </summary>
        private void ProcessIPCData(ref IpcPacket ipcData)
        {
            // IPC opcode
            var ipcOpcode = (int)Offsets.IpcType + 2 < ipcData.Metadata.PacketSize ? BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcType) : new ushort();
            ipcData.Opcode = ipcOpcode;
            if (ipcData.Metadata.ConnectionType == "receive")
            {
                ipcData.Type = ActiveOpcodeList.Lists.ServerZoneIpcType.FirstOrDefault(oi => oi.Opcode == ipcOpcode)?.Name;

                // ActorControl categories
                if (ipcData.Type == "ActorControl" || ipcData.Type == "ActorControlSelf" || ipcData.Type == "ActorControlTarget")
                {
                    var actorControlOpcode = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
                    ipcData.ActorControlCategory = Enum.GetName(typeof(ActorControlType), actorControlOpcode) ?? "unknown";
                    // Camelcase it for JavaScript style
                    Util.JSify(ref ipcData.ActorControlCategory);
                }
            }
            else
            {
                ipcData.Type = ActiveOpcodeList.Lists.ClientZoneIpcType.FirstOrDefault(oi => oi.Opcode == ipcOpcode)?.Name;

                // ClientTrigger categories
                if (ipcData.Type == "ClientTrigger")
                {
                    var clientTriggerOpcode = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
                    ipcData.ClientTriggerCategory = Enum.GetName(typeof(ClientTriggerType), clientTriggerOpcode) ?? "unknown";
                    Util.JSify(ref ipcData.ClientTriggerCategory);
                }
            }

            // Server ID and timestamp
            ipcData.ServerId = (int)Offsets.ServerId + 2 < ipcData.Metadata.PacketSize ? BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.ServerId) : new ushort();
            ipcData.Timestamp = (int)Offsets.Timestamp + 4 < ipcData.Metadata.PacketSize ? BitConverter.ToUInt32(ipcData.Metadata.Data, (int)Offsets.Timestamp) : new uint();

            // ValidateRegion(ipcData);
        }

        /// <summary>
        /// Switches the parser region if it detects an out-of-range world.
        /// </summary>
        private void ValidateRegion(IpcPacket ipcData)
        {
            // Check world ID for region validation
            if (ipcData.Type == "InitZone")
            {
                var worldId = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
                if (Region != Region.Global && worldId >= 23 && worldId <= 99)
                {
                    Region = Region.Global;
                }
                else if (Region != Region.KR && worldId >= 2050 && worldId <= 2583)
                {
                    Region = Region.KR;
                }
                else if (Region != Region.CN && worldId >= 1040 && worldId <= 1671)
                {
                    Region = Region.CN;
                }
            }
        }
    }
}
