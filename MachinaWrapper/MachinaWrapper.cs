/*
 * The purpose of this program is to wrap the information from Machina into a JSON, and to read the header data of the packets.
 * Packet data processing is done in Node.
 */

using Machina;
using Machina.FFXIV;
using Sapphire.Network.Packets;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace MachinaWrapper
{
    class MachinaWrapper
    {
        // https://github.com/SapphireServer/Sapphire/blob/master/src/common/Network/CommonNetwork.h
        const byte PACKET_SIZE_OFFSET = 0x00;
        const byte SOURCE_ACTOR_OFFSET = 0x04;
        const byte TARGET_ACTOR_OFFSET = 0x08;
        const byte SEGMENT_TYPE_OFFSET = 0x0C;
        const byte IPC_TYPE_OFFSET = 0x12;
        const byte SERVER_ID_OFFSET = 0x16;
        const byte TIMESTAMP_OFFSET = 0x18;
        const byte IPC_DATA_OFFSET = 0x20;

        static int Capacity = 0;

        static readonly uint Modulator = (uint) new Random().Next(int.MinValue, int.MaxValue);
        
        static void Main(string[] args)
        {
            // Use arguments to configure the monitor.
            int MonitorIndex = Array.IndexOf(args, "--MonitorType");
            int PIDIndex = Array.IndexOf(args, "--ProcessID");
            int IPIndex = Array.IndexOf(args, "--LocalIP");

            TCPNetworkMonitor.NetworkMonitorType MonitorType = TCPNetworkMonitor.NetworkMonitorType.RawSocket;
            if (MonitorIndex != -1 && args[MonitorIndex + 1] == "WinPCap")
            {
                MonitorType = TCPNetworkMonitor.NetworkMonitorType.WinPCap;
            }

            // Create the monitor.
            FFXIVNetworkMonitor monitor = new FFXIVNetworkMonitor
            {
                MonitorType = MonitorType,
                ProcessID = PIDIndex != -1 ? uint.Parse(args[PIDIndex + 1]) : 0,
                LocalIP = IPIndex != -1 ? args[IPIndex + 1] : "",
                UseSocketFilter = Array.IndexOf(args, "--UseSocketFilter") != -1 ? true : false,
                MessageReceived = (string connection, long epoch, byte[] message) => MessageReceived(connection, epoch, message),
                MessageSent = (string connection, long epoch, byte[] message) => MessageSent(connection, epoch, message)
            };

            // Check for input.
            string input = "";

            // Get the input without blocking the output.
            Thread InputLoop = new Thread(() =>
            {
                while (true)
                {
                    input = Console.In.ReadLine(); // This blocks the InputLoop thread, so there's no need to sleep or anything like that.
                }
            });
            InputLoop.Start();

            // Process the input.
            while (input != "kill")
            {
                if (input == "start")
                {
                    monitor.Start();
                }
                else if (input == "stop")
                {
                    try
                    {
                        monitor.Stop();
                    }
                    catch (NullReferenceException nre) // _monitor is null, and it's a private member of monitor so I can't check if it exists beforehand.
                    {
                        Console.Error.WriteLine(nre);
                    }
                }

                input = "";
                Thread.Sleep(200); // One-fifth of a second is probably fine for user input, and it's way less intensive than 1.
            }

            try
            {
                monitor.Stop();
            }
            catch (NullReferenceException) {}
            InputLoop.Abort();
        }

        /// <summary>
        /// Executes on message receipt. Prints a JSON object to the console.
        /// </summary>
        private static void MessageReceived(string connection, long epoch, byte[] message)
        {
            ParseJSON(connection, epoch, message, "receive");
        }

        /// <summary>
        /// Executes on message send. Prints a JSON object to the console.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] message)
        {
            ParseJSON(connection, epoch, message, "send");
        }

        /// <summary>
        /// Structures the data into a JSON string, and does some preliminary byte parsing.
        /// </summary>
        private static void ParseJSON(string connection, long epoch, byte[] data, string operation)
        {
            // Checking endianness
            if (!BitConverter.IsLittleEndian)
            {
                data = data.Reverse().ToArray();
            }

            // Get packet size.
            uint packetSize = BitConverter.ToUInt32(data, PACKET_SIZE_OFFSET);

            // Actor ID obfuscation for some measure of privacy, discouraging stalking and the like.
            // Ideally this data could just be wiped out, but that would make IPC data completely useless.
            byte[] sourceActorID = BitConverter.GetBytes(BitConverter.ToUInt32(data, SOURCE_ACTOR_OFFSET) ^ Modulator);
            byte[] targetActorID = BitConverter.GetBytes(BitConverter.ToUInt32(data, TARGET_ACTOR_OFFSET) ^ Modulator);
            // Copy obfuscated data in
            for (int i = 0; i < 4; i++)
            {
                data[SOURCE_ACTOR_OFFSET + i] = sourceActorID[i];
                data[TARGET_ACTOR_OFFSET + i] = targetActorID[i];
            }

            // Get IPC data, if applicable.
            ushort segmentType = BitConverter.ToUInt16(data, SEGMENT_TYPE_OFFSET);
            string ipcType = null;
            ushort serverID = 0;
            uint timestamp = 0;
            if (segmentType == 3) // IPC segment type
            {
                // IPC opcode
                if (operation == "receive")
                {
                    // Inbound packet
                    ipcType = Enum.GetName(typeof(ServerZoneIpcType), BitConverter.ToUInt16(data, IPC_TYPE_OFFSET));
                }
                else
                {
                    // Outbound packet
                    ipcType = Enum.GetName(typeof(ClientZoneIpcType), BitConverter.ToUInt16(data, IPC_TYPE_OFFSET));
                }

                // Server ID and timestamp
                serverID = BitConverter.ToUInt16(data, SERVER_ID_OFFSET);
                timestamp = BitConverter.ToUInt32(data, TIMESTAMP_OFFSET);
            }
            
            string type = ipcType ?? "unknown"; // Check if the property name exists
            type = type.Substring(0, 1).ToLower() + type.Substring(1); // Camelcase it for JavaScript style

            // The JSON consists of potentially useful header information and the IPC data if it exists.
            // Heavy data processing is done on the Node side, since it's easier to test packet structures like that.
            StringBuilder JSON = new StringBuilder(Capacity);
            JSON.Append("{ \"type\": \"").Append(type).Append("\",\n");
            JSON.Append("  \"connection\": \"").Append(connection).Append("\",\n");
            JSON.Append("  \"operation\": \"").Append(operation).Append("\",\n");
            JSON.Append("  \"epoch\": ").Append(epoch).Append(",\n");
            JSON.Append("  \"packetSize\": ").Append(packetSize).Append(",\n");
            JSON.Append("  \"segmentType\": ").Append(segmentType).Append(",\n");
            if (segmentType == 3)
            {
                // IPC header info
                JSON.Append("  \"sourceActorSessionID\": ").Append(BitConverter.ToUInt32(data, SOURCE_ACTOR_OFFSET)).Append(",\n");
                JSON.Append("  \"targetActorSessionID\": ").Append(BitConverter.ToUInt32(data, TARGET_ACTOR_OFFSET)).Append(",\n");
                JSON.Append("  \"serverID\": ").Append(serverID).Append(",\n");
                JSON.Append("  \"timestamp\": ").Append(timestamp).Append(",\n");

                if (ipcType != "unknown") // We do need header data if the type is unknown
                {
                    // Trim useless information
                    Array.Copy(data, IPC_DATA_OFFSET, data, 0, packetSize - IPC_DATA_OFFSET);
                }
            }
            JSON.Append("  \"data\": [").Append(string.Join(",", data)).Append("] }\n");

            // Update the size so that future StringBuilders don't need to repeatedly resize themselves.
            if (JSON.Capacity > Capacity)
            {
                Capacity = JSON.Capacity;
            }
            
            Console.Out.Write(JSON.ToString());
        }
    }
}
