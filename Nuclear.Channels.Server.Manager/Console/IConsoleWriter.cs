using System;

namespace Nuclear.Channels.Server.Manager.Console
{
    public interface IConsoleWriter
    {
        void WriteUrls();
        void WriteHelp();
        void Write(string line);
        void TextColor(ConsoleColor color);
        void InjectServerMessagePrefix();
        void InjectServerPrefix();
    }
}
