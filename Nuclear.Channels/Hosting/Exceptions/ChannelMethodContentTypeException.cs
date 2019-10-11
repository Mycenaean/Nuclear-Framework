using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    public class ChannelMethodContentTypeException : Exception
    {
        public ChannelMethodContentTypeException() { }
        public ChannelMethodContentTypeException(string message) : base(message) { }
        public ChannelMethodContentTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
