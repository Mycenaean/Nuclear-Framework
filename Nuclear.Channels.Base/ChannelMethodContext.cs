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
            Request = request;
            Response = response;
            HttpMethod = method;
            IsAuthenticated = isAuthenticated;
            Parameters = parameters;
        }

        public HttpListenerRequest Request { get; }
        public HttpListenerResponse Response { get; }
        public ChannelHttpMethod HttpMethod { get; }
        public List<object> Parameters { get; }
        public bool IsAuthenticated { get; }

    }
}
