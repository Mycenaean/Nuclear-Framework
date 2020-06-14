using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager
{
    public class ChannelServer 
    {
        public static IChannelServerManager GetManager()
        {
            return new ServerManager();
        }
    }
}
