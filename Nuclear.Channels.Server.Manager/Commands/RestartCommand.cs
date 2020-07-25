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
            _writer.Write($"{this.GetType().Name} started execution");
            Task.Run(()=> _methodHandler.Restart());
            Thread.Sleep(1000);
        }

        protected override void ExecuteOnChannel()
        {
            var methods = _channelHandler.MethodHandlers.AsArray();
            foreach (var handler in methods)
            {
                Task.Run(()=> handler.Restart());
            }
        }
    }

}
