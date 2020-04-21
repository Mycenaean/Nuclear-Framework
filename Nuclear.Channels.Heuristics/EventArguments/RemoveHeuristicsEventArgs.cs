// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics.EventArguments
{
    public class RemoveHeuristicsEventArgs : EventArgs
    {
        public HeuristicsInfo SingleForRemoval { get; set; }
        public HeuristicsInfo[] CollectionForRemoval { get; set; }
    }
}
