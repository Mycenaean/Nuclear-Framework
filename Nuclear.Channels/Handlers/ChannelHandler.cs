// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator;
using System;

namespace Nuclear.Channels.Handlers
{
    /// <summary>
    /// Channel specific handler 
    /// </summary>
    /// <remarks>Usage is found in the server</remarks>
    public class ChannelHandler : IHandlerContract
    {
        private readonly IChannelMethodHandlerCollection _methodHandlers;

        public string HandlerId { get; private set; }
        public EntityState State { get; set; }

        public IChannelMethodHandlerCollection MethodHandlers
        {
            get
            {
                return _methodHandlers.GetChannelMethods(HandlerId);
            }
        }

        public ChannelHandler(string name)
        {
            HandlerId = $"{name}_{Guid.NewGuid()}";

            _methodHandlers = ServiceLocatorBuilder
                .CreateServiceLocator()
                .Get<IChannelMethodHandlerCollection>();
        }

        public void Start()
        {
            State = EntityState.Starting;
            ChannelMethodHandler[] handlers = _methodHandlers.AsArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i].Start();
            }
            State = EntityState.Running;
        }

        public void Restart()
        {
            State = EntityState.Restarting; ChannelMethodHandler[] handlers = _methodHandlers.AsArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i].Restart();
            }
            State = EntityState.Running;
        }

        public void Stop()
        {
            ChannelMethodHandler[] handlers = _methodHandlers.AsArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i].Stop();
            }
            State = EntityState.Stopped;
        }
    }
}
