// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager.Commands;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Console
{
    public interface IConsoleReader
    {
        ServerCommandContext Read(string consoleLine);
    }
}
