// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Net;

namespace Nuclear.Channels.Authentication
{
    /// <summary>
    /// Contract for Channel and ChannelMethods Authentication 
    /// </summary>
    public interface IChannelAuthenticationService
    {
        /// <summary>
        /// Authenticationenticating Request based on AuthenticationType
        /// </summary>
        /// <param name="context">HttpListenerContext</param>
        /// <param name="response">HttpListenerResponse for the client</param>
        /// <param name="channelSchema">AuthenticationSchemes for the Channel</param>
        /// <param name="Authenticationenticated">True to be returned if user is Authenticationenticated and Authenticationorized</param>
        bool CheckAuthentication(ChannelAuthenticationContext context);
    }
}
