// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base.Enums;
using System.Collections.Generic;
using System.Net;

namespace Nuclear.Channels.Base
{
    public class ChannelMethodContext : IChannelMethodContext
    {
        public ChannelMethodContext(HttpListenerRequest request, HttpListenerResponse response, ChannelHttpMethod method, List<object> parameters, bool isAuthenticated)
        {
            ChannelMethodRequest = request;
            ChannelMethodResponse = response;
            ChannelHttpMethod = method;
            IsAuthenticated = isAuthenticated;
            Parameters = parameters;
        }

        public HttpListenerRequest ChannelMethodRequest { get; }
        public HttpListenerResponse ChannelMethodResponse { get; }
        public ChannelHttpMethod ChannelHttpMethod { get; }
        public List<object> Parameters { get; }
        public bool IsAuthenticated { get; }

    }
}
