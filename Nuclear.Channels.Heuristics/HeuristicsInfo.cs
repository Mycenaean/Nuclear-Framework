using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    public class HeuristicsInfo
    {
        public Type Channel { get; set; }
        public MethodInfo ChannelMethod { get; set; }
        public List<object> Parameters { get; set; }
        public object MethodResponse { get; set; }
        public DateTime AddedTime { get; set; }
        public int Duration { get; set; }
        public CacheDurationUnit DurationUnit { get; set; }

        public bool Expired()
        {
            if (DurationUnit == CacheDurationUnit.Seconds)
                return AddedTime.AddSeconds(Duration) > DateTime.Now;
            else if(DurationUnit == CacheDurationUnit.Minutes)
                return AddedTime.AddMinutes(Duration) > DateTime.Now;
            else 
                return AddedTime.AddHours(Duration) > DateTime.Now;
        }
    }
}
