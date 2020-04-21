// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

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
