// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Net;

namespace Nuclear.Channels.Messaging
{
    [Export(typeof(IChannelMessageOutputWriter), ExportLifetime.Scoped)]
    internal class ChannelMessageOutputWriter : IChannelMessageOutputWriter
    {
        private readonly IChannelMessageService _msgService;
        private readonly IServiceLocator _services;
        public event EventHandler OnPostMessageServiceInvoked;

        public ChannelMessageOutputWriter()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _msgService = _services.Get<IChannelMessageService>();
        }

        public void Write(IChannelMessage message, HttpListenerResponse response)
        {
            _msgService.WriteHttpResponse(message, response);
            OnPostMessageServiceInvoked?.Invoke(this, EventArgs.Empty);
        }
    }
}
