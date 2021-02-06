using Machina;
using System;
using MachinaWrapper.Common;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace MachinaWrapper
{
    public static class MachinaWrapper
    {

        private static uint Port = 13346U;
        private static readonly HttpClient http = new HttpClient();

        public static async Task Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            var MonitorIndex = Array.IndexOf(args, "--MonitorType");
            var PIDIndex = Array.IndexOf(args, "--ProcessID");
            var IPIndex = Array.IndexOf(args, "--LocalIP");
            var RegionIndex = Array.IndexOf(args, "--Region");
            var PortIndex = Array.IndexOf(args, "--Port");
            var TestIndex = Array.IndexOf(args, "--Test");

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
        }

        /// <summary>
        /// Executes on message receipt.
        /// </summary>
        private static void MessageReceived(string connection, long epoch, byte[] data)
        {
            SendViaHttp('S', data);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] data)
        {
            SendViaHttp('C', data);
        }

        private static void SendViaHttp(char origin, byte[] data)
        {
            byte[] content = new byte[data.Length + 1];
            switch (origin)
            {
                case 'C':
                    content[0] = 0x01;
                    break;
                case 'S':
                    content[0] = 0x02;
                    break;
                default:
                    content[0] = 0x00;
                    break;
            }
            Array.Copy(data, 0, content, 1, data.Length);
            var message = new ByteArrayContent(content);
            http.PostAsync("http://localhost:" + Port, message);
        }
    }
}
