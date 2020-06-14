// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Handlers
{
    public interface IChannelMethodHandlerCollection : IHandlerCollectionContract<ChannelMethodHandler>
    {
        IChannelMethodHandlerCollection GetChannelMethods(string channelHandlerId);
    }
}

