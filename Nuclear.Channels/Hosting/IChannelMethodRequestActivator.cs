// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels.Hosting
{
    /// <summary>
    /// Service which will activate ChannelMethod
    /// </summary>
    public interface IChannelMethodRequestActivator
    {

        /// <summary>
        /// Activate the ChannelMethod which takes input parameters based on post input body
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="channelRequestBody">List of parameters to be initialized</param>
        /// <param name="methodDescription">List of parameter names with their types</param>
        /// <param name="request">Incoming HttpListenerRequest</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
        /// <exception cref="Exceptions.ChannelMethodParameterException">Parameters dont match</exception>
        /// <exception cref="Exceptions.ChannelMethodContentTypeException">Unsupported content type</exception>
        void PostActivate(Type channel, MethodInfo method, List<object> channelRequestBody, Dictionary<string, Type> methodDescription, HttpListenerRequest request, HttpListenerResponse response);

        /// <summary>
        ///  Activate the ChannelMethod which takes input parameters based on query string 
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="channelRequestBody">List of parameters to be initialized</param>
        /// <param name="methodDescription">List of parameter names with their types</param>
        /// <param name="request">Incoming HttpListenerRequest</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
        void GetActivateWithParameters(Type channel, MethodInfo method, List<object> channelRequestBody, Dictionary<string, Type> methodDescription, HttpListenerRequest request, HttpListenerResponse response);

        /// <summary>
        /// Activate the ChannelMethod which takes no input parameters
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
        void GetActivateWithoutParameters(Type channel, MethodInfo method, HttpListenerResponse response);
    }
}

