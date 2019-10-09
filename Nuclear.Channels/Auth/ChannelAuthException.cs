using System;

namespace Nuclear.Channels.Auth
{
    public class ChannelAuthException : Exception
    {
        public ChannelAuthException() { }

        public ChannelAuthException(string message) : base(message) { }
    }
}
