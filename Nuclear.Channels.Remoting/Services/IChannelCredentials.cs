using System.Net;

namespace Nuclear.Channels.Remoting.Services
{
    public interface IChannelCredentials
    {
        AuthenticationSchemes AuthSchema { get; set; }
        string Username { get; set; }
        string Password { get; set; }

    }
}

