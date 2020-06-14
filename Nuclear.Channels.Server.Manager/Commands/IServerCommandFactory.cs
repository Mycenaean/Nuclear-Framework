namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface IServerCommandFactory
    {
        IServerCommand GetCommand(ServerCommandContext context);
    }
}
