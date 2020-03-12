// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Base
{
    public interface IChannelMethodContext
    {
        HttpListenerRequest ChannelMethodRequest { get; }
        HttpListenerResponse ChannelMethodResponse { get; }
        ChannelHttpMethod ChannelHttpMethod { get; }
        List<object> Parameters { get; }
        bool IsAuthenticated { get; }
    }
}
