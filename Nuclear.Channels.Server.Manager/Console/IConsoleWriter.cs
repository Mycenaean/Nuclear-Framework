// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

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
