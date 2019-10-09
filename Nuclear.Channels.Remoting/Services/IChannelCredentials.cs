using System.Net;

namespace Nuclear.Channels.Remoting.Services
{
    public interface IChannelCredentials
    {
        AuthenticationSchemes AuthenticationType { get; set; }
        string Username { get; set; }
        string Password { get; set; }

    }
}
