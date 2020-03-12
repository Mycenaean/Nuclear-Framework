// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Base
{
    /// <summary>
    /// Base abstract Channel Helper class
    /// </summary>
    public class ChannelBase : IChannel
    {
        /// <summary>
        /// ServiceLocator
        /// </summary>
        public IServiceLocator Services => ServiceLocatorBuilder.CreateServiceLocator();

        /// <summary>
        /// Request context
        /// </summary>
        public IChannelMethodContext Context => Services.Get<IChannelMethodContextProvider>().GetDefaultContext();

        /// <summary>
        /// Service that will write IChannelMessage as an output. This is the fastest way to get response from ChannelMethod.
        /// </summary>
        public IChannelMessageOutputWriter ChannelMessageWriter => Services.Get<IChannelMessageOutputWriter>();
    }
}
