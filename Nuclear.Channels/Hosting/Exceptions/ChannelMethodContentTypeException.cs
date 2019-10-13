using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Exceptions
{
    /// <summary>
    /// Exception thrown when Content-Type is neither application/json nor application/xml
    /// </summary>
    public class ChannelMethodContentTypeException : Exception
    {
        public ChannelMethodContentTypeException() { }
        public ChannelMethodContentTypeException(string message) : base(message) { }
        public ChannelMethodContentTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
