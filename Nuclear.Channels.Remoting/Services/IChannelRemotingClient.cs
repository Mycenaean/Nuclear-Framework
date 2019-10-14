using Nuclear.Channels.Remoting.Enums;
using System.Net;

namespace Nuclear.Channels.Remoting.Services
{/// <summary>
 /// Remoting Service to call Channels
 /// </summary>
    public interface IChannelRemotingClient
    {
        /// <summary>
        /// Invoking parameterless ChannelMethod
        /// </summary>
        /// <typeparam name="T">Type of the response object</typeparam>
        /// <param name="URL">URL to ChannelMethod</param>
        /// <param name="method">HttpMethod to invoke</param>
        /// <param name="credentials">Credentials for authorization</param>
        /// <exception cref="Exceptions.ChannelRemotingException">Exception thrown when IChannelMessage Status is false</exception>
        /// <returns>IChannelMessage in form of a string</returns>
        T InvokeChannel<T>(string URL, HttpChannelMethod method, IChannelCredentials credentials);

        /// <summary>
        /// Invoking ChannelMethod with parameters
        /// </summary>
        /// <typeparam name="T">Type of the response object</typeparam>
        /// <param name="URL">URL to ChannelMethod</param>
        /// <param name="bodyInput">Object to be sent as a parameter</param>
        /// <param name="method">HttpMethod to invoke</param>
        /// <param name="credentials">Credentials for authorization</param>
        /// <param name="contentType">Input body content-type</param>
        /// <exception cref="Exceptions.ChannelRemotingException">Exception thrown when IChannelMessage Status is false</exception>
        /// <returns>IChannelMessage in form of a string</returns>
        T InvokeChannel<T>(string URL, object inputBody, HttpChannelMethod method, IChannelCredentials credentials, string contentType = null);
    }

}
