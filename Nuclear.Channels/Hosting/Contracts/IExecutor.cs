// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Reflection;
using System.Threading;

namespace Nuclear.Channels.Hosting.Contracts
{
    /// <summary>
    /// Service Contract for ChannelActivator to initialize HttpEndpoints
    /// </summary>
    internal interface IExecutor
    {
        /// <summary>
        /// Method that will get all ChannelMethods from inspected Channel
        /// </summary>
        /// <param name="channel">Inspected Channel</param>
        void MethodExecute(Type channel, CancellationToken token);

        /// <summary>
        /// Method that is doing all the heavy lifting, Http endpoint initialization for specified ChannelMethod
        /// </summary>
        /// <param name="method">ChannelMethod to be initialized as Http Endpoint</param>
        /// <param name="channel">Web Channel</param>
        void StartListening(MethodInfo method, Type channel, CancellationToken token);


    }
}
