// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;

namespace Nuclear.Channels.Server.Manager.Commands
{
    [Export(typeof(IServerCommandInterpreter), ExportLifetime.Singleton)]
    public class ServerCommandInterpreter : IServerCommandInterpreter
    {
        private IChannelHandlerCollection _channelHandlers;

        public ServerCommandInterpreter()
        {
            _channelHandlers = ServiceLocatorBuilder
                .CreateServiceLocator()
                .Get<IChannelHandlerCollection>()
                ?? throw new ArgumentNullException(nameof(_channelHandlers));
        }

        public ServerCommandTarget InterpretTarget(string handlerId)
        {
            if (_channelHandlers.GetHandler(handlerId) == null)
                return ServerCommandTarget.ChannelMethod;
            else
                return ServerCommandTarget.Channel;
        }

        public ServerCommandType InterpretType(string type)
        {
            switch(type.ToLower())
            {
                case "restart":
                    return ServerCommandType.Restart;
                case "start":
                    return ServerCommandType.Start;
                case "stop":
                    return ServerCommandType.Stop;
                case "read":
                    return ServerCommandType.Read;
                default:
                    throw new ArgumentException(nameof(type));
            }
        }
    }
}
