// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace Nuclear.Channels.Authentication
{
    /// <summary>
    /// Contract for Channel and ChannelMethods Authentication 
    /// </summary>
    public interface IChannelAuthenticationService
    {
        /// <summary>
        /// Authenticating request based on AuthenticationType
        /// </summary>
        /// <param name="context">HttpListenerContext</param>
        /// <param name="response">HttpListenerResponse for the client</param>
        /// <param name="channelSchema">AuthenticationSchemes for the Channel</param>
        /// <param name="Authenticationenticated">True to be returned if user is Authenticationenticated and Authenticationorized</param>
        [Obsolete("Please use CheckAuthenticationAndGetResponseObject method")]
        bool CheckAuthentication(ChannelAuthenticationContext context);

        /// <summary>
        /// Authenticating request based on AuthenticationContext
        /// </summary>
        /// <param name="authContext">Composed ChannelAuthenticationContext</param>
        /// <exception cref="ChannelCredentialsException"></exception>
        /// <returns>KeyValuePair containing boolean indicating is authentication successful and if yes response object of a delegate responsible for validation</returns>
        KeyValuePair<bool, object> CheckAuthenticationAndGetResponseObject(ChannelAuthenticationContext authContext);

        /// <summary>
        /// Checks authorization based on claim name and claim value
        /// </summary>
        /// <param name="claimName">Name of the claim</param>
        /// <param name="claimValue">Value of the specified claim</param>
        /// <param name="principal">Current claims principal</param>
        /// <returns>Boolean indicating if user is authorized</returns>
        bool Authorized(string claimName, string claimValue, ClaimsPrincipal principal);

        /// <summary>
        /// Checks authorization based on claim name and claim value
        /// </summary>
        /// <param name="claimName">Name of the claim</param>
        /// <param name="claimValue">Value of the specified claim</param>
        /// <param name="claims">Current claims for a specified user</param>
        /// <returns>Boolean indicating if user is authorized</returns>
        bool Authorized(string claimName, string claimValue, Claim[] claims);


    }
}
