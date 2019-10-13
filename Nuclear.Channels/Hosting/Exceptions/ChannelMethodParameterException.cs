using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    /// <summary>
    /// Exception thrown when parameters dont match
    /// </summary>
    public class ChannelMethodParameterException : Exception
    {
        public ChannelMethodParameterException() { }
        public ChannelMethodParameterException(string message) : base(message) { }
        public ChannelMethodParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
