using Nuclear.Channels.Remoting.Enums;
using Nuclear.Channels.Remoting.Services;

namespace Nuclear.Channels.Remoting
{
    /// <summary>
    /// RemoteInvoker Service
    /// </summary>
    internal interface IRemoteInvoker
    {
        /// <summary>
        /// Invoking parameterless ChannelMethod
        /// </summary>
        /// <param name="URL">URL to ChannelMethod</param>
        /// <param name="method">HttpMethod to invoke</param>
        /// <param name="credentials">Credentials for authorization</param>
        /// <returns>IChannelMessage in form of a string</returns>
        string InvokeChannel(string URL, HttpChannelMethod method, IChannelCredentials credentials);

        /// <summary>
        /// Invoking ChannelMethod with parameters
        /// </summary>
        /// <param name="URL">URL to ChannelMethod</param>
        /// <param name="bodyInput">Object to be sent as a parameter</param>
        /// <param name="method">HttpMethod to invoke</param>
        /// <param name="credentials">Credentials for authorization</param>
        /// <param name="contentType">Input body content-type</param>
        /// <returns>IChannelMessage in form of a string</returns>
        string InvokeChannel(string URL, string bodyInput, HttpChannelMethod method, IChannelCredentials credentials, string contentType = null);

    }
}
