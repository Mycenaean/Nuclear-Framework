using System;

namespace Nuclear.Channels.Remoting.Exceptions
{
    public class ChannelRemotingException : Exception
    {
        public ChannelRemotingException() { }
        public ChannelRemotingException(string message) : base(message) { }
        public ChannelRemotingException(string message, Exception inner) : base(message, inner) { }
    }
}
