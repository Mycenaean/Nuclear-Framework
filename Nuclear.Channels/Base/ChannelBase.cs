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
        /// Redirect to a specified url, https prefix is the default if not provided otherwise
        /// </summary>
        /// <param name="url">Specified url to redirect the response</param>
        /// <param name="isHttps">Url Schema, default is https</param>
        public void RedirectToUrl(string url , bool isHttps = true)
        {
            if (!isHttps)
                RedirectToHttpUrl(url);

            if (CheckForHttpPrefix(url))
                _events.ExecuteRedirection(url, Context.ChannelMethodResponse);
            else
                _events.ExecuteRedirection($"https://{url}", Context.ChannelMethodResponse);
        }

        private void RedirectToHttpUrl(string url)
        {
            if (CheckForHttpPrefix(url))
                _events.ExecuteRedirection(url, Context.ChannelMethodResponse);
            else
                _events.ExecuteRedirection($"http://{url}", Context.ChannelMethodResponse);
        }

        private bool CheckForHttpPrefix(string url)
        {
            string prefix = $"{url[0]}{url[1]}{url[2]}{url[3]}";
            return prefix == "http";
        }
    }
}
