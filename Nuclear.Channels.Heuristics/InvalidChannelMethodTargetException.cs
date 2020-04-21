using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Heuristics
{
    /// <summary>
    /// Exception thrown when EnableCacheAttribute is applied to method returning void
    /// </summary>
    public class InvalidChannelMethodTargetException : Exception
    {
        public InvalidChannelMethodTargetException() { }
        public InvalidChannelMethodTargetException(string message) : base(message) { }
    }
}
