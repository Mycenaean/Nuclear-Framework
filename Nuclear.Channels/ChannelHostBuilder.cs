using Nuclear.Channels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels
{
    /// <summary>
    /// Class that contains method for building channel host
    /// </summary>
    public class ChannelHostBuilder
    {
        /// <summary>
        /// Method that creates IChannelHost instance
        /// </summary>
        public static IChannelHost CreateHost()
        {
            return ChannelHost.GetHost;
        }
    }
}
