// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class ReadStateCommand : ServerCommand, IServerCommand
    {
        public ReadStateCommand(ChannelHandler channelHandler) : base(channelHandler, ConsoleColor.Green)
        {
        }

        public ReadStateCommand(ChannelMethodHandler methodHandler) : base(methodHandler, ConsoleColor.Green)
        {
        }

        protected override void ExecuteOnChannel()
        {
            _result = _channelHandler.State;
        }

        protected override void ExecuteOnMethod()
        {
            _result = _methodHandler.State;
        }
    }

}
