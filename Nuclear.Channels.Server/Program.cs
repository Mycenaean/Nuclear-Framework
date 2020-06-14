using Nuclear.Channels.Server.Manager;
using System;

namespace Nuclear.Channels.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IChannelServerManager serverManager = ChannelServer.GetManager();

            IChannelServer server = serverManager.Server;
            server.LoadAssemblies(AppDomain.CurrentDomain, null);
            server.IsServerManaged(true);
            server.StartHosting(null);

            IServerInstructionCollection instructions = serverManager.Instructions;
            instructions.PrintOnConsole();

            while (true)
            {
                //print all channels and channel methods with their ids and states
                string instruction = Console.ReadLine();
                //interpret instruction
                //execute instruction
            }
            
        }
    }
}
