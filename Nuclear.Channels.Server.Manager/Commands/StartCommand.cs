// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Nuclear.Channels.Handlers;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class StartCommand : ServerCommand, IServerCommand
    {
        public StartCommand(ChannelHandler channelHandler) : base(channelHandler, ConsoleColor.Green)
        {
        }

        public StartCommand(ChannelMethodHandler methodHandler) : base(methodHandler, ConsoleColor.Green)
        {
        }

        protected override void ExecuteOnMethod()
        {
            _writer.Write($"{this.GetType().Name} started execution");
            Task.Run(() => _methodHandler.Start());
            Thread.Sleep(1000);
        }

        protected override void ExecuteOnChannel()
        {
            _writer.Write($"{this.GetType().Name} started execution");
            ChannelMethodHandler[] methods = _channelHandler.MethodHandlers.AsArray();
            for (int i = 0; i < methods.Length; i++)
            {
                methods[i].Start();
            }
        }
    }

}
