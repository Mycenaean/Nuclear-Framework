using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Manager
{
    [Export(typeof(IServerInstructionCollection), ExportLifetime.Singleton)]
    internal class ServerInstructionCollection : IServerInstructionCollection
    {
        private const string lineWrapper = "============================================";
        private const string startChannel = "start channel [channelId]";
        private const string restartChannel = "restart channel [channelId]";
        private const string stopChannel = "stop channel [channelId]";
        private const string startChannelMethod = "start channelMethod [channelMethodId]";
        private const string restartChannelMethod = "restart channelMethod [channelMethodId]";
        private const string stopChannelMethod = "stop channelMethod [channelMethodId]";
        private const string readChannelState = "read channel state [channelId]";
        private const string readChannelMethodState = "read channel state [channelMethodId]";
        private const string readChannelMethodUpTime = "read channelmethod uptime [channelMethodId]";

        public List<string> Instructions { get; private set; }

        public ServerInstructionCollection()
        {
            Instructions = new List<string>();
            InitInstructions();
        }

        public void PrintOnConsole()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(lineWrapper);
            Console.WriteLine("List of possible instructions");
            Console.WriteLine();
            foreach (string instruction in Instructions)
            {
                Console.WriteLine(instruction);
            }
            Console.WriteLine();
            Console.WriteLine(lineWrapper);
        }

        private void InitInstructions()
        {
            Instructions.Add(startChannel);
            Instructions.Add(restartChannel);
            Instructions.Add(stopChannel);
            Instructions.Add(startChannelMethod);
            Instructions.Add(restartChannelMethod);
            Instructions.Add(stopChannelMethod);
            Instructions.Add(readChannelState);
            Instructions.Add(readChannelMethodState);
            Instructions.Add(readChannelMethodUpTime);
        }
    }
}
