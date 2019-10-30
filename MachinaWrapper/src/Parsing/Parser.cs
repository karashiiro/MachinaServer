using MachinaWrapper.Common;
using Sapphire.Common.ActorControl;
using Sapphire.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NameSizePair = System.Collections.Generic.KeyValuePair<string, int>;

namespace MachinaWrapper.Parsing
{
    public class Parser
    {
        public int Capacity;
        public List<NameSizePair> MessageSizes;
        public ParserMode Mode;
        public Region Region;

        private readonly uint Modulator = (uint)new Random().Next(int.MinValue, int.MaxValue);

        public Parser(Region region)
        {
            Capacity = 50; // Initialize the StringBuilder with 50 characters.
            MessageSizes = new List<NameSizePair>();
            Mode = ParserMode.RAMHeavy;
            Region = region;
        }

        public Parser(Region region, ParserMode mode)
        {
            Capacity = 50;
            MessageSizes = new List<NameSizePair>();
            Mode = mode;
            Region = region;
        }

        public Parser(Region region, ParserMode mode, int capacity)
        {
            Capacity = capacity;
            MessageSizes = new List<NameSizePair>();
            Mode = mode;
            Region = region;
        }

        public Parser(Region region, ParserMode mode, List<NameSizePair> capacities)
        {
            Capacity = 50;
            MessageSizes = capacities;
            Mode = mode;
            Region = region;
        }

        /// <summary>
        /// Structures the data into a JSON string, and does some preliminary byte parsing.
        /// </summary>
        public void Parse(Packet meta)
        {
            // Checking endianness
            if (!BitConverter.IsLittleEndian)
            {
                meta.Data = meta.Data.Reverse().ToArray();
            }

            // Get packet size.
            meta.PacketSize = BitConverter.ToUInt32(meta.Data, (int)Offsets.PacketSize);

            // Actor ID obfuscation for some measure of privacy, discouraging stalking and the like.
            // Ideally this data could just be wiped out, but that would make IPC data useless for minimap features.
            byte[] sourceActorID = (int)Offsets.SourceActor + 4 < meta.PacketSize ? BitConverter.GetBytes(BitConverter.ToUInt32(meta.Data, (int)Offsets.SourceActor) ^ Modulator) : new byte[4];
            byte[] targetActorID = (int)Offsets.TargetActor + 4 < meta.PacketSize ? BitConverter.GetBytes(BitConverter.ToUInt32(meta.Data, (int)Offsets.TargetActor) ^ Modulator) : new byte[4];
            // Copy obfuscated data in
            for (int i = 0; i < 4; i++)
            {
                meta.Data[(int)Offsets.SourceActor + i] = sourceActorID[i];
                meta.Data[(int)Offsets.TargetActor + i] = targetActorID[i];
            }

            // Get IPC data, if applicable.
            meta.SegmentType = (int)Offsets.SegmentType + 2 < meta.PacketSize ? BitConverter.ToUInt16(meta.Data, (int)Offsets.SegmentType) : new ushort();
            IpcPacket ipcData = new IpcPacket(meta);
            if (meta.SegmentType == 3) // IPC segment type
            {
                ProcessIPCData(ref ipcData);
            }

            ipcData.Type = ipcData.Type ?? "unknown"; // Check if the property name exists
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
            }
        }

        /// <summary>
        /// Structures the data into a JSON string, prints it to the console. This uses a StringBuilder sized to the largest packet recieved.
        /// This can use quite a bit of RAM and can crash low-memory systems if many packets are being parsed at once. Modern computers can run this happily.
        /// </summary>
        private void OutputRAMHeavy(IpcPacket ipcData)
        {
            // This is just a slight modification of the CPU-heavy logger.
            StringBuilder sb = OutputCPUHeavy(ipcData);

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
            StringBuilder sb = OutputCPUHeavy(ipcData);

            // Update the size so that future StringBuilders don't need to repeatedly resize themselves.
            if (sb.Capacity > ipcMessageSize.Value)
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
            StringBuilder JSON = new StringBuilder(capacity ?? Capacity);
            JSON.Append("{ \"type\": \"").Append(ipcData.Type).Append("\",\n");
            JSON.Append("  \"opcode\": ").Append(ipcData.Opcode).Append(",\n");
            JSON.Append("  \"region\": \"").Append(Region).Append("\",\n");
            JSON.Append("  \"connection\": \"").Append(ipcData.Metadata.ConnectionRoute).Append("\",\n");
            JSON.Append("  \"operation\": \"").Append(ipcData.Metadata.ConnectionType).Append("\",\n");
            JSON.Append("  \"epoch\": ").Append(ipcData.Metadata.Epoch).Append(",\n");
            JSON.Append("  \"packetSize\": ").Append(ipcData.Metadata.PacketSize).Append(",\n");
            JSON.Append("  \"segmentType\": ").Append(ipcData.Metadata.SegmentType).Append(",\n");
            if (ipcData.Metadata.SegmentType == 3)
            {
                // IPC header info
                JSON.Append("  \"sourceActorSessionID\": ").Append(BitConverter.ToUInt32(ipcData.Metadata.Data, (int)Offsets.SourceActor)).Append(",\n");
                JSON.Append("  \"targetActorSessionID\": ").Append(BitConverter.ToUInt32(ipcData.Metadata.Data, (int)Offsets.TargetActor)).Append(",\n");
                JSON.Append("  \"serverID\": ").Append(ipcData.ServerId).Append(",\n");
                JSON.Append("  \"timestamp\": ").Append(ipcData.Timestamp).Append(",\n");

                if (ipcData.Type != "unknown") // We do need header data if the type is unknown
                {
                    // Trim useless information
                    Array.Copy(ipcData.Metadata.Data, (int)Offsets.IpcData, ipcData.Metadata.Data, 0, ipcData.Metadata.PacketSize - (int)Offsets.IpcData);

                    // If the IPC type isn't unknown we might have category data sitting around
                    if (ipcData.ActorControlCategory != null)
                    {
                        JSON.Append("  \"superType\": \"actorControl\",\n");
                        JSON.Append("  \"subType\": \"").Append(ipcData.ActorControlCategory).Append("\",\n");
                    }
                    else if (ipcData.ClientTriggerCategory != null)
                    {
                        JSON.Append("  \"superType\": \"clientTrigger\",\n");
                        JSON.Append("  \"subType\": \"").Append(ipcData.ClientTriggerCategory).Append("\",\n");
                    }
                }
            }
            JSON.Append("  \"data\": [").Append(string.Join(",", ipcData.Metadata.Data)).Append("] }\n");

            Console.Out.Write(JSON.ToString());

            return JSON;
        }

        /// <summary>
        /// Converts opcodes into readable strings.
        /// </summary>
        private void ProcessIPCData(ref IpcPacket ipcData)
        {
            // IPC opcode
            ushort ipcOpcode = (int)Offsets.IpcType + 2 < ipcData.Metadata.PacketSize ? BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcType) : new ushort();
            ipcData.Opcode = ipcOpcode;
            if (ipcData.Metadata.ConnectionType == "receive")
            {
                // Inbound packet
                if (Region == Region.Global)
                {
                    ipcData.Type = Enum.GetName(typeof(ServerZoneIpcType), ipcOpcode);
                    if (ipcData.Type == null)
                    {
                        ipcData.Type = Enum.GetName(typeof(ServerChatIpcType), ipcOpcode);
                    }
                }
                else if (Region == Region.KR)
                {
                    ipcData.Type = Enum.GetName(typeof(ServerZoneIpcTypeKR), ipcOpcode);
                    if (ipcData.Type == null)
                    {
                        ipcData.Type = Enum.GetName(typeof(ServerChatIpcTypeKR), ipcOpcode);
                    }
                }
                else if (Region == Region.CN)
                {
                    //
                }
                else
                {
                    throw new NoRegionException("No region set!");
                }

                // ActorControl categories
                if (ipcData.Type == "ActorControl" || ipcData.Type == "ActorControlSelf" || ipcData.Type == "ActorControlTarget")
                {
                    ushort actorControlOpcode = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
                    ipcData.ActorControlCategory = Enum.GetName(typeof(ActorControlType), actorControlOpcode) ?? "unknown";
                    // Camelcase it for JavaScript style
                    Util.JSify(ref ipcData.ActorControlCategory);
                }
            }
            else
            {
                // Outbound packet
                if (Region == Region.Global)
                {
                    ipcData.Type = Enum.GetName(typeof(ClientZoneIpcType), ipcOpcode);
                    if (ipcData.Type == null)
                    {
                        ipcData.Type = Enum.GetName(typeof(ClientChatIpcType), ipcOpcode);
                    }
                }
                else if (Region == Region.KR)
                {
                    ipcData.Type = Enum.GetName(typeof(ClientZoneIpcTypeKR), ipcOpcode);
                    if (ipcData.Type == null)
                    {
                        ipcData.Type = Enum.GetName(typeof(ClientChatIpcTypeKR), ipcOpcode);
                    }
                }
                else if (Region == Region.CN)
                {
                    //
                }
                else
                {
                    throw new NoRegionException("No region set!");
                }

                // ClientTrigger categories
                if (ipcData.Type == "ClientTrigger")
                {
                    ushort clientTriggerOpcode = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
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
                ushort worldId = BitConverter.ToUInt16(ipcData.Metadata.Data, (int)Offsets.IpcData);
                if (Region != Region.Global && 23 >= worldId && worldId <= 99)
                {
                    Region = Region.Global;
                }
                else if (Region != Region.KR && 2050 >= worldId && worldId <= 2583)
                {
                    Region = Region.KR;
                }
                // TODO China
            }
        }
    }
}
