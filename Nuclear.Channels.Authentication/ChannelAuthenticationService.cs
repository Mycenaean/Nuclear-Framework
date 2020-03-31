// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication.Identity;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Data.Services;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Nuclear.Channels.Authentication
{
    [Export(typeof(IChannelAuthenticationService), ExportLifetime.Transient)]
    public class ChannelAuthenticationService : IChannelAuthenticationService
    {
        [Obsolete("Please use CheckAuthenticationAndGetResponseObject method")]
        public bool CheckAuthentication(ChannelAuthenticationContext authContext)
        {
            HttpListenerIdentityService identityService = new HttpListenerIdentityService(authContext.BasicAuthenticationDelegate, authContext.TokenAuthenticationDelegate);

            bool knownUser = identityService.AuthenticatedAndAuthorized(authContext.Context, authContext.Scheme);
            return knownUser;
        }


        public KeyValuePair<bool, object> CheckAuthenticationAndGetResponseObject(ChannelAuthenticationContext authContext)
        {
            HttpListenerIdentityService identityService = new HttpListenerIdentityService(authContext);
            bool knownUser = identityService.Authenticated(out object result);
            return new KeyValuePair<bool, object>(knownUser, result);
        }

        public bool Authorized(string claimName, string claimValue, ClaimsPrincipal principal)
        {
            Claim claim =  principal.Claims.FirstOrDefault(x => x.Type.Equals(claimName, StringComparison.OrdinalIgnoreCase));
            return IsClaimValueValid(claim, claimValue);
        }

        public bool Authorized(string claimName, string claimValue, Claim[] claims)
        {
            Claim claim = claims.FirstOrDefault(x => x.Type.Equals(claimName, StringComparison.OrdinalIgnoreCase));
            return IsClaimValueValid(claim, claimValue);
        }

        private bool IsClaimValueValid(Claim claim,string claimValue)
        {
            if (claim == null)
                return false;
            if (claim.Value.Equals(claimValue, StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }
    }
}
