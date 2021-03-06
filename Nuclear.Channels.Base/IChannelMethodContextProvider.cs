﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base.Contracts;

namespace Nuclear.Channels.Base
{
    public interface IChannelMethodContextProvider
    {
        void SetChannelMethodContext(IChannelEndpoint endpoint, IChannelMethodContext context);
        void DestroyChannelMethodContext(IChannelEndpoint endpoint);
        IChannelMethodContext GetChannelMethodContext(IChannelEndpoint endpoint);
        IChannelMethodContext GetDefaultContext();
    }
}