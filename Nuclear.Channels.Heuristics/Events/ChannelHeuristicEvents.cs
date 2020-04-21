// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Nuclear.Channels.Heuristics.EventArguments;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;

namespace Nuclear.Channels.Heuristics.Events
{
    [Export(typeof(IChannelHeuristicEvents), ExportLifetime.Scoped)]
    public class ChannelHeuristicEvents : IChannelHeuristicEvents
    {
        public event EventHandler<AddHeuristicsEventArgs> AddToHeuristics;
        public event EventHandler<RemoveHeuristicsEventArgs> RemoveFromHeuristics;

        public void OnAddToHeuristics(AddHeuristicsEventArgs args)
        {
            AddToHeuristics?.Invoke(this, args);
        }

        public void OnRemoveFromHeuristics()
        {
            throw new NotImplementedException();
        }
    }
}
