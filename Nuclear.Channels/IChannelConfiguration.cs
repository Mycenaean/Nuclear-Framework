using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels
{
    internal interface IChannelConfiguration
    {
        ChannelConfigurationInfo Configure(HttpListener httpChannel, Type channel, MethodInfo method, string baseURL);
    }
}
