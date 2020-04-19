using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.EventArguments
{
    public class AddHeuristicsEventArgs : EventArgs
    {
        public Type Channel { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public List<object> Parameters { get; set; }
        public object MethodResponse { get; set; }
    }

}

