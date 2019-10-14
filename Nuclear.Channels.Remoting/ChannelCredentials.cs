using Nuclear.Channels.Remoting.Services;
using System.Net;

namespace Nuclear.Channels.Remoting
{
    public class ChannelCredentials : IChannelCredentials
    {
        public AuthenticationSchemes AuthSchema { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

