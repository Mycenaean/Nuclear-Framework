using System.Security.Claims;

namespace Nuclear.Channels.Server.Web.Authentication
{
    public interface IAuthenticationService
    {
        Claim[] AuthenticateUser(string username, string password);
    }

}
