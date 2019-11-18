using System.Net;

namespace Nuclear.Channels.Auth
{
    /// <summary>
    /// Contract for Channel and ChannelMethods Auth 
    /// </summary>
    internal interface IChannelAuthentication
    {
        /// <summary>
        /// Authenticating Request based on AuthType
        /// </summary>
        /// <param name="context">HttpListenerContext</param>
        /// <param name="response">HttpListenerResponse for the client</param>
        /// <param name="ChannelSchema">AuthenticationSchemes for the Channel</param>
        /// <param name="authenticated">True to be returned if user is authenticated and authorized</param>
        void AuthenticateRequest(HttpListenerContext context, HttpListenerResponse response, AuthenticationSchemes ChannelSchema, out bool authenticated);
    }
}
