// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base.Enums;
using System.Collections.Generic;
using System.Net;

namespace Nuclear.Channels.Base
{
    /// <summary>
    /// Service that provide Request-Response Context
    /// </summary>
    /// <remarks>Be careful how you use Request and Response Objects</remarks>
    public interface IChannelMethodContext
    {
        /// <summary>
        /// Http Request sent to ChannelMethod
        /// </summary>
        HttpListenerRequest ChannelMethodRequest { get; }

        /// <summary>
        /// Http Response to be written
        /// </summary>
        /// <remarks>Be careful with what you do with the response since closing the response inside ChannelMethod could break the workflow</remarks>
        HttpListenerResponse ChannelMethodResponse { get; }

        /// <summary>
        /// Http Method used to call ChannelMethod
        /// </summary>
        ChannelHttpMethod ChannelHttpMethod { get; }

        /// <summary>
        /// ChannelMethod parameter list ordered
        /// </summary>
        List<object> Parameters { get; }

        /// <summary>
        /// Is User Authenticated
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
