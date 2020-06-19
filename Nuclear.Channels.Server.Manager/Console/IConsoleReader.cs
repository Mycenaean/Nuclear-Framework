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
