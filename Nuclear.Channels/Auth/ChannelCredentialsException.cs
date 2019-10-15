using System;

namespace Nuclear.Channels.Auth
{
    public class ChannelCredentialsException : Exception
    {
        public ChannelCredentialsException() { }

        public ChannelCredentialsException(string message) : base(message) { }
    }
}
