/*
 * The purpose of this program is to wrap the information from Machina into a JSON, and to read the header data of the packets.
 * Packet data processing is done in Node.
 */

using Machina;
using MachinaWrapper.Common;
using MachinaWrapper.Parsing;
using System;
using System.Threading;

namespace MachinaWrapper
{
    class MachinaWrapper
    {
        static Parser Parser;

        static void Main(string[] args)
        {
            // Configure the monitor with command-line arguments.
            int MonitorIndex = Array.IndexOf(args, "--MonitorType");
            int PIDIndex = Array.IndexOf(args, "--ProcessID");
            int IPIndex = Array.IndexOf(args, "--LocalIP");
            int RegionIndex = Array.IndexOf(args, "--Region");
            int PortIndex = Array.IndexOf(args, "--Port");
            
            TCPNetworkMonitor.NetworkMonitorType MonitorType = TCPNetworkMonitor.NetworkMonitorType.RawSocket;
            if (MonitorIndex != -1 && args[MonitorIndex + 1] == "WinPCap")
            {
                MonitorType = TCPNetworkMonitor.NetworkMonitorType.WinPCap;
            }

            Region localRegion = Region.Global;
            if (RegionIndex != -1)
            {
                if (args[RegionIndex + 1] == "KR")
                {
                    localRegion = Region.KR;
                }
                else if (args[RegionIndex + 1] == "CN")
                {
                    localRegion = Region.CN;
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

            // Create the monitor.
            FFXIVNetworkMonitor monitor = new FFXIVNetworkMonitor
            {
                MonitorType = MonitorType,
                Region = localRegion,
                ProcessID = PIDIndex != -1 ? uint.Parse(args[PIDIndex + 1]) : 0,
                LocalIP = IPIndex != -1 ? args[IPIndex + 1] : "",
                UseSocketFilter = Array.IndexOf(args, "--UseSocketFilter") != -1 ? true : false,
                MessageReceived = (string connection, long epoch, byte[] message) => MessageReceived(connection, epoch, message),
                MessageSent = (string connection, long epoch, byte[] message) => MessageSent(connection, epoch, message)
            };

            // Create the parser.
            int ParseAlgorithmIndex = Array.IndexOf(args, "--ParseAlgorithm");
            if (ParseAlgorithmIndex != -1)
            {
                if (args[ParseAlgorithmIndex + 1] == "RAMHeavy")
                {
                    Parser = new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1]));
                }
                else if (args[ParseAlgorithmIndex + 1] == "CPUHeavy")
                {
                    Parser = new Parser(localRegion, ParserMode.CPUHeavy, uint.Parse(args[PortIndex + 1]));
                }
                else if (args[ParseAlgorithmIndex + 1] == "PacketSpecific")
                {
                    Parser = new Parser(localRegion, ParserMode.PacketSpecific, uint.Parse(args[PortIndex + 1]));
                }
                else
                {
                    Parser = new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1]));
                }
            }
            else
            {
                Parser = new Parser(localRegion, ParserMode.RAMHeavy, uint.Parse(args[PortIndex + 1]));
            }

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
            Thread InputProcessingLoop = new Thread(() => {
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
            Packet meta = new Packet(connection, "receive", epoch, data);
            Parser.Parse(meta);
        }

        /// <summary>
        /// Executes on message send.
        /// </summary>
        private static void MessageSent(string connection, long epoch, byte[] data)
        {
            Packet meta = new Packet(connection, "send", epoch, data);
            Parser.Parse(meta);
        }
    }
}
