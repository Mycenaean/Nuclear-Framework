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
    public abstract class ChannelBase : IChannel
    {
        private readonly IChannelRedirectionEvents _events;

        /// <summary>
        /// ServiceLocator
        /// </summary>
        public IServiceLocator Services { get; }

        /// <summary>
        /// Request context
        /// </summary>
        public IChannelMethodContext Context { get; }

        /// <summary>
        /// Service that will write IChannelMessage as an output. This is the fastest way to get response from ChannelMethod.
        /// </summary>
        public IChannelMessageOutputWriter ChannelMessageWriter { get; }

        public ChannelBase()
        {
            Services = ServiceLocatorBuilder.CreateServiceLocator();
            Context = Services.Get<IChannelMethodContextProvider>().GetDefaultContext();
            ChannelMessageWriter = Services.Get<IChannelMessageOutputWriter>();
            _events = Services.Get<IChannelRedirectionEvents>();
        }

        /// <summary>
        /// Redirect to a specified url
        /// </summary>
        /// <param name="url">Specified url to redirect the response</param>
        public void RedirectToUrl(string url)
        {
            _events.ExecuteRedirection(url, Context.ChannelMethodResponse);
        }
    }
}
