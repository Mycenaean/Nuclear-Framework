using Nuclear.Channels.Remoting.Enums;

namespace Nuclear.Channels.Remoting.Services
{
    public interface IChannelCredentials
    {
        ChannelAuthenticationOptions AuthenticationOptions { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Token { get; set; }

    }
}

