using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Manager
{
    public interface IServerInstructionCollection
    {
        List<string> Instructions { get; }
        void PrintOnConsole();
    }
}
