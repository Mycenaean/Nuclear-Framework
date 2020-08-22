// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.Settings;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Linq;
using System.Security.Claims;

namespace Nuclear.Channels.Server.Web.Authentication
{
    [Export(typeof(IAuthenticationService), ExportLifetime.Scoped)]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IWebServerSettings _serverSettings;

        public AuthenticationService()
        {
            _serverSettings = ServiceFactory.GetExportedService<IWebServerSettings>();
        }

        public Claim[] AuthenticateUser(string username, string password)
        {
            var user = _serverSettings.Users.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(password));
            var Claims = new Claim[1];
            if (user == null)
                return null;

            Claims[0] = new Claim("Role", user.Role);
            return Claims;
        }
    }

}
