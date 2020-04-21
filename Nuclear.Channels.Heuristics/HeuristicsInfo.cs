// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Entity containing all informations for caching
    /// </summary>
    public class HeuristicsInfo
    {
        public Type Channel { get; set; }
        public MethodInfo ChannelMethod { get; set; }
        public List<object> Parameters { get; set; }
        public object MethodResponse { get; set; }
        public DateTime AddedTime { get; set; }
        public double Duration { get; set; }
        public CacheDurationUnit DurationUnit { get; set; }

        public bool Expired()
        {
            DateTime cacheDuration;
            if (DurationUnit == CacheDurationUnit.Seconds)
                cacheDuration = AddedTime.AddSeconds(Duration);
            else if (DurationUnit == CacheDurationUnit.Minutes)
                cacheDuration = AddedTime.AddMinutes(Duration);
            else
                cacheDuration = AddedTime.AddHours(Duration);

            int expired = DateTime.Compare(DateTime.Now, cacheDuration);
            return expired > 0;
        }
    }
}
