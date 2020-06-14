using Nuclear.Channels.Handlers;
using System;
using System.Drawing;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public abstract class ServerCommand
    {
        protected ChannelMethodHandler _methodHandler;
        protected ChannelHandler _channelHandler;
        protected bool _isMethod;

        public ConsoleColor Color { get; }

        public ServerCommand(ChannelHandler channelHandler, ConsoleColor color)
        {
            _channelHandler = channelHandler;
            Color = color;
            _isMethod = false;
        }

        public ServerCommand(ChannelMethodHandler methodHandler, ConsoleColor color)
        {
            _methodHandler = methodHandler;
            Color = color;
            _isMethod = true;
        }

        public void Execute()
        {
            if (_isMethod)
                ExecuteOnMethod();
            else
                ExecuteOnChannel();
        }

        protected abstract void ExecuteOnMethod();
        protected abstract void ExecuteOnChannel();

    }

}
