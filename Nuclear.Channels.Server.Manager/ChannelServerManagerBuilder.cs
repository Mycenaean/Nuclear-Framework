using Nuclear.ExportLocator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager
{
    public class ChannelServerManagerBuilder
    {
        public static IChannelServerManager Build(Action<IChannelServer> serverAction)
        {
            IChannelServer server = ChannelServerBuilder.CreateServer();
            serverAction(server);
            return new ServerManager(ServiceLocatorBuilder.CreateServiceLocator(), server);
        }
    }
}
