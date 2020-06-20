// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Decorators;
using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Channels
{
    /// <summary>
    /// API in case Server Manager is non Console Application
    /// </summary>
    [Channel]
    public class ServerChannel : ChannelBase
    {
        private readonly IChannelMethodHandlerCollection _methodHandlers;
        private readonly IChannelHandlerCollection _channelHandlers;
        private readonly IServerCommandFactory _commandFactory;

        public ServerChannel()
        {
            _methodHandlers = Services.Get<IChannelMethodHandlerCollection>();
            _channelHandlers = Services.Get<IChannelHandlerCollection>();
            _commandFactory = Services.Get<IServerCommandFactory>();
        }

        /// <summary>
        /// Get the List of all ChannelHandlers
        /// </summary>
        [ChannelMethod]
        public List<ChannelHandler> ListChannels()
        {
            return _channelHandlers.AsList();
        }

        /// <summary>
        /// Get the List of all ChannelMethodHandlers
        /// </summary>
        [ChannelMethod]
        public List<ChannelMethodHandler> ListChannelMethods()
        {
            return _methodHandlers.AsList();
        }

        /// <summary>
        /// Start (Channel or ChannelMethod) Handler
        /// </summary>
        /// <param name="HandlerId">Handler ID</param>
        [ChannelMethod]
        public void Start(string HandlerId)
        {
            ExecuteCommand(HandlerId, "start");
        }

        /// <summary>
        /// Stop (Channel or ChannelMethod) Handler
        /// </summary>
        /// <param name="HandlerId">Handler ID</param>
        [ChannelMethod]
        public void Restart(string HandlerId)
        {
            ExecuteCommand(HandlerId, "restart");
        }

        /// <summary>
        /// Stop (Channel or ChannelMethod) Handler
        /// </summary>
        /// <param name="HandlerId">Handler ID</param>
        [ChannelMethod]
        public void Stop(string HandlerId)
        {
            ExecuteCommand(HandlerId, "stop");
        }

        /// <summary>
        /// Execute ServerCommand based on Command Type
        /// </summary>
        /// <param name="handlerId">Handler for which command is to be executed</param>
        /// <param name="command">Type of the command to be executed</param>
        private void ExecuteCommand(string handlerId, string command)
        {
            ServerCommandContext commandContext = new ServerCommandContext(Services, handlerId, command);
            IServerCommand serverCommand = _commandFactory.GetCommand(commandContext);
            serverCommand.Execute();
        }
    }
}
