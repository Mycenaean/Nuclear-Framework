// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Handlers
{
    [Export(typeof(IChannelMethodHandlerCollection), ExportLifetime.Singleton)]
    internal class ChannelMethodHandlerCollection : IChannelMethodHandlerCollection
    {
        private List<ChannelMethodHandler> _handlers;

        public ChannelMethodHandlerCollection()
        {
            _handlers = new List<ChannelMethodHandler>();
        }

        private ChannelMethodHandlerCollection(List<ChannelMethodHandler> handlers)
        {
            _handlers = handlers;
        }


        public void AddHandler(ChannelMethodHandler handler)
        {
            _handlers.Add(handler);
        }

        public ChannelMethodHandler GetHandler(string HandlerId)
        {
            return _handlers.FirstOrDefault(x => x.HandlerId.Equals(HandlerId));
        }

        public void RemoveHandler(ChannelMethodHandler handler)
        {
            _handlers.Remove(handler);
        }

        public IChannelMethodHandlerCollection GetChannelMethods(string channelHandlerId)
        {
            List<ChannelMethodHandler> channelHandlers = _handlers.Where(x => x.ChannelHandlerId.Equals(channelHandlerId)).ToList();

            return new ChannelMethodHandlerCollection(channelHandlers);            
        }

        public List<ChannelMethodHandler> AsList()
        {
            return _handlers;
        }

        public ChannelMethodHandler[] AsArray()
        {
            return _handlers.ToArray();
        }
    }
}

