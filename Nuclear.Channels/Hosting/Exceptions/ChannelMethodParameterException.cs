using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    public class ChannelMethodParameterException : Exception
    {
        public ChannelMethodParameterException() { }
        public ChannelMethodParameterException(string message) : base(message) { }
        public ChannelMethodParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
