// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Handlers
{
    [Export(typeof(IChannelHandlerCollection), ExportLifetime.Singleton)]
    internal class ChannelHandlerCollection : IChannelHandlerCollection
    {
        private List<ChannelHandler> _handlers;

        public ChannelHandlerCollection()
        {
            _handlers = new List<ChannelHandler>();
        }

        public void AddHandler(ChannelHandler handler)
        {
            _handlers.Add(handler);
        }

        public ChannelHandler GetHandler(string HandlerId)
        {
            return _handlers.FirstOrDefault(x => x.HandlerId.Equals(HandlerId));
        }

        public void RemoveHandler(ChannelHandler handler)
        {
            _handlers.Remove(handler);
        }
        public List<ChannelHandler> AsList()
        {
            return _handlers;
        }
        public ChannelHandler[] AsArray()
        {
            return _handlers.ToArray();
        }

    }
}
