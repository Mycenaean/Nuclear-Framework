using Nuclear.Channels.Remoting.Enums;
using Nuclear.Channels.Remoting.Services;
using System.Net;

namespace Nuclear.Channels.Remoting
{
    public class ChannelCredentials : IChannelCredentials
    {
        public ChannelCredentials()
        {

        }

        public ChannelCredentials(ChannelAuthenticationOptions options, string token)
        {
            AuthenticationOptions = options;
            Token = token;
        }

        public ChannelCredentials(ChannelAuthenticationOptions options, string username, string password)
        {
            AuthenticationOptions = options;
            Username = username;
            Password = password;
        }

        public ChannelAuthenticationOptions AuthenticationOptions { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}

