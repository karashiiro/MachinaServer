using Machina;
using MachinaWrapper.Common;
using MachinaWrapper.Parsing;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Sapphire.Common.Packets;

namespace MachinaWrapper
{
    public static class MachinaWrapper
    {
        private static Parser _parser;

        public static async Task Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            var MonitorIndex = Array.IndexOf(args, "--MonitorType");
            var PIDIndex = Array.IndexOf(args, "--ProcessID");
            var IPIndex = Array.IndexOf(args, "--LocalIP");
            var RegionIndex = Array.IndexOf(args, "--Region");
            var PortIndex = Array.IndexOf(args, "--Port");
            var TestIndex = Array.IndexOf(args, "--Test");

            if (TestIndex != -1)
            {
                ValidateOpcodes();
                return;
            }

            var port = 13346U;
            if (PortIndex != -1)
            {
                port = uint.Parse(args[PortIndex + 1]);
            }

            var MonitorType = TCPNetworkMonitor.NetworkMonitorType.RawSocket;
            if (MonitorIndex != -1 && args[MonitorIndex + 1] == "WinPCap")
            {
                MonitorType = TCPNetworkMonitor.NetworkMonitorType.WinPCap;
            }

            var localRegion = Region.Global;
            if (RegionIndex != -1)
            {
                localRegion = args[RegionIndex + 1] switch
                {
                    "KR" => Region.KR,
                    "CN" => Region.CN,
                    _ => localRegion,
                };
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

            var commander = new Commander("kill");
            commander.AddCommand("start", monitor.Start);
            commander.AddCommand("stop", () =>
            {
                try
                {
                    monitor.Stop();
                }
                catch (NullReferenceException nre) // _monitor is null, and it's a private member of monitor so I can't check if it exists beforehand.
                {
                    Console.Error.WriteLine(nre);
                }
            });
            commander.OnKill(() =>
            {
                try
                {
                    monitor.Stop();
                }
                catch (NullReferenceException) { }
            });
            commander.Start();

            /*var server = new WebSocketServer(int.Parse(args[PortIndex + 1]));
            server.AddWebSocketService("/", () => ParseServer.Create(commander));
            server.Start();*/

            // Create the parser.
            var ParseAlgorithmIndex = Array.IndexOf(args, "--ParseAlgorithm");
            if (ParseAlgorithmIndex != -1)
            {
                _parser = args[ParseAlgorithmIndex + 1] switch
                {
                    "RAMHeavy" => new Parser(localRegion, ParserMode.RAMHeavy, port),
                    "CPUHeavy" => new Parser(localRegion, ParserMode.CPUHeavy, port),
                    "PacketSpecific" => new Parser(localRegion, ParserMode.PacketSpecific, port),
                    _ => new Parser(localRegion, ParserMode.RAMHeavy, port),
                };
            }
            else
            {
                _parser = new Parser(localRegion, ParserMode.RAMHeavy, port);
            }

            await _parser.Initialize();
        }

        /// <summary>
        /// Executes on message receipt.
        /// </summary>
        private static void MessageReceived(string connection, long epoch, byte[] data)
        {
            var meta = new Packet(connection, "receive", epoch, data);
            _parser?.Parse(meta);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] data)
        {
            var meta = new Packet(connection, "send", epoch, data);
            _parser?.Parse(meta);
        }

        private static void ValidateOpcodes()
        {
            var globalIpcLists = new[] { typeof(ClientChatIpcType), typeof(ServerChatIpcType), typeof(ClientLobbyIpcType), typeof(ServerLobbyIpcType), typeof(ClientZoneIpcType), typeof(ServerZoneIpcType) };
            var cnIpcLists = new[] { typeof(ClientChatIpcTypeCN), typeof(ServerChatIpcTypeCN), typeof(ClientZoneIpcTypeCN), typeof(ServerZoneIpcTypeCN) };
            var krIpcLists = new[] { typeof(ClientChatIpcTypeKR), typeof(ServerChatIpcTypeKR), typeof(ClientLobbyIpcTypeKR), typeof(ServerLobbyIpcTypeKR), typeof(ClientZoneIpcTypeKR), typeof(ServerZoneIpcTypeKR) };
            var ipcLists = new[] { globalIpcLists, cnIpcLists, krIpcLists};

            foreach (var ipcList in ipcLists)
            {
                foreach (var ipcType in ipcList)
                {
                    var ipcValues = (ushort[])Enum.GetValues(ipcType);
                    if (ipcValues.Distinct().Count() != ipcValues.Length)
                        throw new ConfigurationErrorsException(
                            $"{ipcType.Name} contains one or more duplicate values!");
                }
            }
        }
    }
}
