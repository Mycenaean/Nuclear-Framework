using Nuclear.Channels.Contracts;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
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

        internal void SetChannelMethodContext(IChannelEndpoint endpoint , IChannelMethodContext context)
        {
            _contexts.Add(endpoint, context);
        }

        internal void DestroyChannelMethodContext(IChannelEndpoint endpoint)
        {
            _contexts.Remove(endpoint);
        }

        internal IChannelMethodContext GetChannelMethodContext(IChannelEndpoint endpoint)
        {
            return _contexts.GetValueOrDefault(endpoint);
        }
    }
}
