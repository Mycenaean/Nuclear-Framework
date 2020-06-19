using Nuclear.Channels.Server.Manager;
using System;

namespace Nuclear.Channels.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IChannelServerManager serverManager = ChannelServerManagerBuilder.Build(server => 
            {
                server.LoadAssemblies(AppDomain.CurrentDomain, null);
                server.IsServerManaged(true);
            });

            serverManager.Start();            
        }
    }
}

