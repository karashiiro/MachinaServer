using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;

namespace MachinaWrapper
{
    internal sealed class PacketDispatcher
    {
        private readonly string requestUri;
        private readonly HttpClient http = new HttpClient();

        private readonly ConcurrentQueue<byte[]> packetQueue = new ConcurrentQueue<byte[]>();

        private ulong packetCnt = 0;
        private readonly object cntLock = new object();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Thread thread;

        public PacketDispatcher(string requestUri)
        {
            this.requestUri = requestUri;
        }

        public void EnqueuePacket(MessageSource origin, byte[] data)
        {
            lock (this.cntLock)
            {
                var content = new byte[data.Length + 1 + 8];

                content[0] = origin switch
                {
                    MessageSource.Client => 0x01,
                    MessageSource.Server => 0x02,
                    _ => 0x00,
                };

                var packetIndexBuffer = BitConverter.GetBytes(this.packetCnt);
                Array.Copy(packetIndexBuffer, 0, content, 1, packetIndexBuffer.Length);
                Array.Copy(data, 0, content, 9, data.Length);

                this.packetCnt++;

                this.packetQueue.Enqueue(content);
            }
        }

        public void Start()
        {
            this.thread = new Thread(this.Run);
            this.thread.Start();
        }

        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
            this.thread.Join();
        }

        private async void Run()
        {
            while (!this.cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (this.packetQueue.TryDequeue(out var sendPacket))
                {
                    using var message = new ByteArrayContent(sendPacket);
                    await this.http.PostAsync(this.requestUri, message);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}