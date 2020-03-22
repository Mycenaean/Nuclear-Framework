// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Enums;
using System;
using System.IO;
using System.Net;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Service responsible for the writing ChannelMessage output to the client
    /// </summary>
    public interface IChannelMessageService
    {
        /// <summary>
        /// Method that will write response to the client
        /// </summary>
        /// <param name="response">HttpListenerResponse instance</param>
        /// <param name="chResponse">ChannelMethod response object</param>
        void WriteHttpResponse(object chResponse, HttpListenerResponse response);

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
        void FailedAuthenticationResponse(ChannelAuthenticationSchemes ChannelSchema, HttpListenerResponse response);

        /// <summary>
        /// Wrong HttpMethod Used
        /// </summary>
        /// <param name="response">Response for the client</param>
        /// <param name="HttpMethod">Required HttpMethod</param>
        void WrongHttpMethod(HttpListenerResponse response, ChannelHttpMethod HttpMethod);
    }
}
