using System;
using System.Collections.Generic;
using System.Threading;

namespace MachinaWrapper
{
    public class Commander
    {
        private Thread inputLoop;
        private Thread inputProcessingLoop;

        private readonly IDictionary<string, Action> commands;

        private readonly string killCommand;
        private Action killAction;

        public Commander(string killCommand)
        {
            commands = new Dictionary<string, Action>();
            this.killCommand = killCommand;
        }

        public void AddCommand(string command, Action action)
        {
            commands.Add(command, action);
        }

        public void OnKill(Action action)
        {
            killAction = action;
        }

        public void InvokeCommand(string command)
        {
            if (commands.TryGetValue(command, out var action))
            {
                action();
            }
        }

        public void Start()
        {
            // Check for input.
            var input = "";

            // Get the input without blocking the output.
            inputLoop = new Thread(() =>
            {
                while (true)
                {
                    input = Console.ReadLine(); // This blocks the inputLoop thread, so there's no need to sleep or anything like that.
                }
            });
            inputLoop.Start();

            // Process the input.
            inputProcessingLoop = new Thread(() => {
                while (input != killCommand)
                {
                    InvokeCommand(input);
                    input = "";
                    Thread.Sleep(200); // One-fifth of a second is probably fine for user input, and it's way less intensive than 1.
                }

                killAction();
                inputLoop.Abort();
            });
            inputProcessingLoop.Start();
        }

        public void Stop()
        {
            if (inputLoop.IsAlive)
                inputLoop.Abort();

            if (inputProcessingLoop.IsAlive)
                inputProcessingLoop.Abort();
        }
    }
}
