using Machina;
using System;
using MachinaWrapper.Common;


namespace MachinaWrapper
{
    public static class MachinaWrapper
    {

        private static uint Port = 13346U;
        private static ulong CurrentPacketIndex = 0U;
        private static PacketDispatcher PacketDispatcher;

        public static void Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            var MonitorIndex = Array.IndexOf(args, "--MonitorType");
            var PIDIndex = Array.IndexOf(args, "--ProcessID");
            var IPIndex = Array.IndexOf(args, "--LocalIP");
            var RegionIndex = Array.IndexOf(args, "--Region");
            var PortIndex = Array.IndexOf(args, "--Port");

            if (PortIndex != -1)
            {
                Port = uint.Parse(args[PortIndex + 1]);
            }

            var MonitorType = TCPNetworkMonitor.NetworkMonitorType.WinPCap;
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

            PacketDispatcher = new PacketDispatcher("http://localhost:" + Port);

            var commander = new Commander("kill");
            commander.AddCommand("start", () =>
            {
                PacketDispatcher.Start();
                monitor.Start();
            });
            commander.AddCommand("stop", () =>
            {
                try
                {
                    monitor.Stop();
                    PacketDispatcher.Cancel();
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
                    PacketDispatcher.Cancel();
                }
                catch (NullReferenceException) { }
            });
            commander.Start();
        }

        /// <summary>
        /// Executes on message receipt.
        /// </summary>
        private static void MessageReceived(string connection, long epoch, byte[] data)
        {
            PacketDispatcher.EnqueuePacket(MessageSource.Server, data);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] data)
        {
            PacketDispatcher.EnqueuePacket(MessageSource.Client, data);
        }
    }
}
