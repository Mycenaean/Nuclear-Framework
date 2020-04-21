using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    public class ChannelMethodHeuristicOptions
    {
        public Type Channel { get; set; }
        public MethodInfo ChannelMethod { get; set; }
        public HttpListenerRequest Request { get; set; }
        public HttpListenerResponse Response { get; set; }

    }
}
