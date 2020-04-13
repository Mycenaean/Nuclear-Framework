using System;

namespace Nuclear.Channels.Remoting
{
    public class ChannelRequestException : Exception
    {
        public ChannelRequestException() { }
        public ChannelRequestException(string message) : base(message) { }
    }
}
