// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Nuclear.Channels.Authentication
{
    /// <summary>
    /// Class which will provide all settings neccessary for authentication
    /// </summary>
    public class AuthenticationSettings
    {
        public AuthenticationSettings()
        {
            Schema = ChannelAuthenticationSchemes.Anonymous;
        }

        /// <summary>
        /// Authentication Schema
        /// </summary>
        public ChannelAuthenticationSchemes Schema { get; set; }

        /// <summary>
        /// Delegate which will return only if user is authenticated based on token
        /// </summary>
        public Func<string, bool> TokenAuthDelegate { get; set; }

        /// <summary>
        /// Delegate which will return ClaimsPrincipal based on token authentication
        /// </summary>
        public Func<string, ClaimsPrincipal> TokenAuthPrincipalDelegate { get; set; }

        /// <summary>
        /// Delegate which will return claims based on token authentication
        /// </summary>
        public Func<string, Claim[]> TokenAuthClaimsDelegate { get; set; }

        /// <summary>
        /// Delegate which will return only if user is authenticated based on user and password
        /// </summary>
        public Func<string, string, bool> BasicAuthDelegate { get; set; }

        /// <summary>
        /// Delegate which will return ClaimsPrincipal based on basic authentication
        /// </summary>
        public Func<string, string, ClaimsPrincipal> BasicAuthPrincipalDelegate { get; set; }

        /// <summary>
        /// Delegate which will return claims based on basic authentication
        /// </summary>
        public Func<string, string, Claim[]> BasicAuthClaimsDelegate { get; set; }

    }
}
