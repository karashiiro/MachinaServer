using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MachinaWrapper
{
    public class ParseServer : WebSocketBehavior
    {
        private static ParseServer _sInstance;

        public static ParseServer Create(Commander commander)
        {
            return _sInstance ??= new ParseServer(commander);
        }

        public static void BroadcastMessage(string message)
        {
            _sInstance.Sessions.Broadcast(message);
        }

        private readonly Commander commander;

        private ParseServer(Commander commander)
        {
            this.commander = commander;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            commander.InvokeCommand(e.Data);
        }
    }
}