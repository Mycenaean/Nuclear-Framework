// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Drawing;
using Nuclear.Channels.Handlers;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class RestartCommand : ServerCommand, IServerCommand
    {
        public RestartCommand(ChannelHandler channelHandler) : base(channelHandler, ConsoleColor.Yellow)
        {
        }

        public RestartCommand(ChannelMethodHandler methodHandler) : base(methodHandler, ConsoleColor.Yellow)
        {
        }

        protected override void ExecuteOnMethod()
        {
            _methodHandler.Restart();
        }

        protected override void ExecuteOnChannel()
        {
            ChannelMethodHandler[] methods = _channelHandler.MethodHandlers.AsArray();
            for (int i = 0; i < methods.Length; i++)
            {
                methods[i].Restart();
            }
        }
    }

}
