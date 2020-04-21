// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Enables cached response
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EnableCacheAttribute : Attribute
    {
        public double Duration { get; set; }
        public CacheDurationUnit Unit { get; set; }

        /// <summary>
        /// Registers cached response in seconds
        /// </summary>
        /// <param name="duration">Number of seconds</param>
        public EnableCacheAttribute(double duration)
        {
            Duration = duration;
            Unit = CacheDurationUnit.Seconds;
        }

        /// <summary>
        /// Registers cached response
        /// </summary>
        /// <param name="duration">Time interval</param>
        /// <param name="unit">Time interval unit</param>
        public EnableCacheAttribute(double duration, CacheDurationUnit unit)
        {
            Duration = duration;
            Unit = unit;
        }
    }
}
