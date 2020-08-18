/*
 * The purpose of this program is to wrap the information from Machina into a JSON, and to read the header data of the packets.
 * Packet data processing is done in Node.
 */

using Machina;
using MachinaWrapper.Common;
using MachinaWrapper.Parsing;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Sapphire.Common.Packets;

namespace MachinaWrapper
{
    class MachinaWrapper
    {
        static Parser Parser;

        static void Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            var MonitorIndex = Array.IndexOf(args, "--MonitorType");
            var PIDIndex = Array.IndexOf(args, "--ProcessID");
            var IPIndex = Array.IndexOf(args, "--LocalIP");
            var RegionIndex = Array.IndexOf(args, "--Region");
            var PortIndex = Array.IndexOf(args, "--Port");

            if (PortIndex == -1)
            {
                Console.WriteLine("Port must be provided via command line");
                Environment.Exit(1);
            }

            var MonitorType = TCPNetworkMonitor.NetworkMonitorType.RawSocket;
            if (MonitorIndex != -1 && args[MonitorIndex + 1] == "WinPCap")
            {
                MonitorType = TCPNetworkMonitor.NetworkMonitorType.WinPCap;
            }

            var localRegion = Region.Global;
            if (RegionIndex != -1)
            {
                switch (args[RegionIndex + 1])
                {
                    case "KR":
                        localRegion = Region.KR;
                        break;
                    case "CN":
                        localRegion = Region.CN;
                        break;
                }
            }
            else if (!Util.SystemHasGlobalClient())
            {
                if (Util.SystemHasKRClient())
                {
                    localRegion = Region.KR;
                }
                else if (Util.SystemHasCNClient())
                {
                    localRegion = Region.CN;
                }
            }

            ValidateOpcodes(localRegion);

            // Create the monitor.
            var monitor = new FFXIVNetworkMonitor
            {
                MonitorType = MonitorType,
                Region = localRegion,
                ProcessID = PIDIndex != -1 ? uint.Parse(args[PIDIndex + 1]) : 0,
                LocalIP = IPIndex != -1 ? args[IPIndex + 1] : "",
                UseSocketFilter = Array.IndexOf(args, "--UseSocketFilter") != -1,
                MessageReceived = MessageReceived,
                MessageSent = MessageSent,
            };

            // Create the parser.
            var ParseAlgorithmIndex = Array.IndexOf(args, "--ParseAlgorithm");
            if (ParseAlgorithmIndex != -1)
            {
                Parser = args[ParseAlgorithmIndex + 1] switch
                {
                    "RAMHeavy" => new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1])),
                    "CPUHeavy" => new Parser(localRegion, ParserMode.CPUHeavy, uint.Parse(args[PortIndex + 1])),
                    "PacketSpecific" => new Parser(localRegion, ParserMode.PacketSpecific,
                        uint.Parse(args[PortIndex + 1])),
                    _ => new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1]))
                };
            }
            else
            {
                Parser = new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1]));
            }

            // Check for input.
            var input = "";

            // Get the input without blocking the output.
            var InputLoop = new Thread(() =>
            {
                while (true)
                {
                    input = Console.In.ReadLine(); // This blocks the InputLoop thread, so there's no need to sleep or anything like that.
                }
            });
            InputLoop.Start();

            // Process the input.
            var InputProcessingLoop = new Thread(() => {
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
            });
            InputProcessingLoop.Start();
        }

        /// <summary>
        /// Executes on message receipt.
        /// </summary>
        private static void MessageReceived(string connection, long epoch, byte[] data)
        {
            var meta = new Packet(connection, "receive", epoch, data);
            Parser.Parse(meta);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] data)
        {
            var meta = new Packet(connection, "send", epoch, data);
            Parser.Parse(meta);
        }

        private static void ValidateOpcodes(Region region)
        {
            var globalIpcLists = new[] { typeof(ClientChatIpcType), typeof(ServerChatIpcType), typeof(ClientLobbyIpcType), typeof(ServerLobbyIpcType), typeof(ClientZoneIpcType), typeof(ServerZoneIpcType) };
            var cnIpcLists = new[] { typeof(ClientChatIpcTypeCN), typeof(ServerChatIpcTypeCN), typeof(ClientZoneIpcTypeCN), typeof(ServerZoneIpcTypeCN) };
            var krIpcLists = new[] { typeof(ClientChatIpcTypeKR), typeof(ServerChatIpcTypeKR), typeof(ClientLobbyIpcTypeKR), typeof(ServerLobbyIpcTypeKR), typeof(ClientZoneIpcTypeKR), typeof(ServerZoneIpcTypeKR) };

            foreach (var ipcList in region switch
            {
                Region.Global => globalIpcLists,
                Region.CN => cnIpcLists,
                Region.KR => krIpcLists,
                _ => throw new NotImplementedException(),
            })
            {
                var ipcValues = (ushort[]) Enum.GetValues(ipcList);
                if (ipcValues.Distinct().Count() != ipcValues.Length)
                    throw new ConfigurationErrorsException(
                        $"{ipcList.Name} contains one or more duplicate values!");
            }
        }
    }
}
