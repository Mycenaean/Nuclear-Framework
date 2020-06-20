using Nuclear.Channels.Server.Manager.CoreCommands;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface IServerCommandFactory
    {
        IServerCommand GetCommand(ServerCommandContext context);
        IServerCommand GetCoreCommand(CoreCommand coreCommand);
    }    
}
