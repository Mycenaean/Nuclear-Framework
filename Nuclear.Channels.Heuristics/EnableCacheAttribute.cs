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
        public int Duration { get; set; }
        public CacheDurationUnit Unit { get; set; }

        /// <summary>
        /// Registers cached response in seconds
        /// </summary>
        /// <param name="duration">Number of seconds</param>
        public EnableCacheAttribute(int duration)
        {
            Duration = duration;
            Unit = CacheDurationUnit.Seconds;
        }

        /// <summary>
        /// Registers cached response
        /// </summary>
        /// <param name="duration">Time interval</param>
        /// <param name="unit">Time interval unit</param>
        public EnableCacheAttribute(int duration, CacheDurationUnit unit)
        {
            Duration = duration;
            Unit = unit;
        }
    }
}
