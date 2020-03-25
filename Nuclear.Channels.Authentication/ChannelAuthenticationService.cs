using Nuclear.Channels.Authentication.Identity;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Data.Services;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Authentication
{
    [Export(typeof(IChannelAuthenticationService), ExportLifetime.Transient)]
    public class ChannelAuthenticationService : IChannelAuthenticationService
    {
        public bool CheckAuthentication(ChannelAuthenticationContext authContext)
        {
            HttpListenerIdentityService identityService = new HttpListenerIdentityService(authContext.BasicAuthenticationDelegate, authContext.TokenAuthenticationDelegate);

            bool knownUser = identityService.AuthenticatedAndAuthorized(authContext.Context, authContext.Scheme);
            return knownUser;
        }
    }
}
