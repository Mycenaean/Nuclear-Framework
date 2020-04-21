// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.Contexts
{
    /// <summary>
    /// Service containing information about ChannelMethod thats cached or expects caching
    /// </summary>
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
