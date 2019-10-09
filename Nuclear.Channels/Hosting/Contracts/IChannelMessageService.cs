using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels.Hosting.Contracts
{
    /// <summary>
    /// Service responsible for the writing ChannelMessage output to the client
    /// </summary>
    internal interface IChannelMessageService
    {
        /// <summary>
        /// Method that will write response to the client
        /// </summary>
        /// <param name="response">HttpListenerResponse instance</param>
        /// <param name="channel">Channel</param>
        /// <param name="method">ChannelMethod</param>
        void WriteHttpResponse(HttpListenerResponse response, Type channel, MethodInfo method, object channelResponse = null);

        /// <summary>
        /// Method that will proccess the Exception
        /// </summary>
        /// <param name="writer">StreamWriter instance</param>
        /// <param name="ex">Thrown Exception</param>
        /// <param name="response">HttpListenerResponse instance that will be sent to the client</param>
        void ExceptionHandler(StreamWriter writer, Exception ex, HttpListenerResponse response);


        /// <summary>
        /// Failed Auth ChannelMessage
        /// </summary>
        /// <param name="ChannelSchema">AuthenticationSchemes schema</param>
        /// <param name="response">Response for the client</param>
        void FailedAuthenticationResponse(AuthenticationSchemes ChannelSchema, HttpListenerResponse response);
    }
}
