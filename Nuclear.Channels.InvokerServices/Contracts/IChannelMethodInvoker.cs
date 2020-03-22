// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels.InvokerServices.Contracts
{
    /// <summary>
    /// Service that will invoke targeted ChannelMethod 
    /// </summary>
    internal interface IChannelMethodInvoker
    {
        /// <summary>
        /// Method that will Invoke targeted ChannelMethod without parameters
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response);

        /// <summary>
        /// Method that will Invoke targeted ChannelMethod
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody);

        /// <summary>
        /// Method that will Invoke targeted Sync ChannelMethod
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        void InvokeChannelMethodSync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody);

        /// <summary>
        /// Method that will Invoke targeted Async ChannelMethod
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        void InvokeChannelMethodAsync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody);
    }
}
