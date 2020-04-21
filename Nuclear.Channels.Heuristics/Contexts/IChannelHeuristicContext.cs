using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.Contexts
{
    public interface IChannelHeuristicContext
    {
        bool ExpectsAdding { get; set; }
        Type Channel { get; set; }
        MethodInfo MethodInfo { get; set; }
        List<object> Parameters { get; set; }
        object MethodResponse { get; set; }

        void CacheResponse();
        void Clear();
    }
}
