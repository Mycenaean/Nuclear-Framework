// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.


using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Nuclear.Channels.Authentication.Extensions
{
    public static class ChannelAuthenticationExtensions
    {
        /// <summary>
        /// Adds Basic Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddBasicAuthentication(this IChannelAuthenticationEnabled server, Func<string, string, bool> basicDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                BasicAuthDelegate = basicDelegate,
                Schema = ChannelAuthenticationSchemes.Basic
            };
            return server;
        }

        /// <summary>
        /// Adds Basic Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddBasicAuthentication(this IChannelAuthenticationEnabled server, Func<string, string, Claim[]> basicDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                BasicAuthClaimsDelegate = basicDelegate,
                Schema = ChannelAuthenticationSchemes.Basic
            };
            return server;

        }

        /// <summary>
        /// Adds Basic Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddBasicAuthentication(this IChannelAuthenticationEnabled server, Func<string, string, ClaimsPrincipal> basicDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                BasicAuthPrincipalDelegate = basicDelegate,
                Schema = ChannelAuthenticationSchemes.Basic
            };
            return server;

        }

        /// <summary>
        /// Adds Token Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddTokenAuthentication(this IChannelAuthenticationEnabled server, Func<string, bool> tokenDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                TokenAuthDelegate = tokenDelegate,
                Schema = ChannelAuthenticationSchemes.Token
            };
            return server;

        }

        /// <summary>
        /// Adds Token Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddTokenAuthentication(this IChannelAuthenticationEnabled server, Func<string, Claim[]> tokenDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                TokenAuthClaimsDelegate = tokenDelegate,
                Schema = ChannelAuthenticationSchemes.Token
            };
            return server;

        }

        /// <summary>
        /// Adds Token Authentication rules
        /// </summary>
        /// <param name="basicDelegate">Delegate used to authenticate user</param>
        public static IChannelAuthenticationEnabled AddTokenAuthentication(this IChannelAuthenticationEnabled server, Func<string, ClaimsPrincipal> tokenDelegate)
        {
            server.AuthenticationSettings = new AuthenticationSettings()
            {
                TokenAuthPrincipalDelegate = tokenDelegate,
                Schema = ChannelAuthenticationSchemes.Token
            };
            return server;

        }
    }
}
