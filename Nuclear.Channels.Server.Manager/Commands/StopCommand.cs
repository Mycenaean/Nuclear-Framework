// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Drawing;
using Nuclear.Channels.Handlers;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class StopCommand : ServerCommand, IServerCommand
    {
        public StopCommand(ChannelHandler channelHandler) : base(channelHandler, ConsoleColor.Red)
        {
        }

        public StopCommand(ChannelMethodHandler methodHandler) : base(methodHandler, ConsoleColor.Red)
        {
        }

        protected override void ExecuteOnMethod()
        {
            _methodHandler.Stop();
        }

        protected override void ExecuteOnChannel()
        {
            ChannelMethodHandler[] methods = _channelHandler.MethodHandlers.AsArray();
            for (int i = 0; i < methods.Length; i++)
            {
                methods[i].Stop();
            }
        }
    }

}
