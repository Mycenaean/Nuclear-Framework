using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.EventArguments
{
    public class RemoveHeuristicsEventArgs : EventArgs
    {
        public Type Channel { get; set; }
        public MethodInfo ChannelMethod { get; set; }
    }
}
