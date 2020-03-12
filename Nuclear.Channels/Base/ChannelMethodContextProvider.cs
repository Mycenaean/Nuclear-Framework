// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Contracts;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuclear.Channels.Base
{
    [Export(typeof(IChannelMethodContextProvider), ExportLifetime.Singleton)]
    internal class ChannelMethodContextProvider : IChannelMethodContextProvider
    {
        private Dictionary<IChannelEndpoint, IChannelMethodContext> _contexts;

        public ChannelMethodContextProvider()
        {
            _contexts = new Dictionary<IChannelEndpoint, IChannelMethodContext>();
        }

        public void SetChannelMethodContext(IChannelEndpoint endpoint , IChannelMethodContext context)
        {
            _contexts.Add(endpoint, context);
        }

        public void DestroyChannelMethodContext(IChannelEndpoint endpoint)
        {
            _contexts.Remove(endpoint);
        }

        public IChannelMethodContext GetChannelMethodContext(IChannelEndpoint endpoint)
        {
            return _contexts.GetValueOrDefault(endpoint);
        }

        public IChannelMethodContext GetDefaultContext()
        {
            return _contexts.Values.FirstOrDefault();
        }
    }
}
