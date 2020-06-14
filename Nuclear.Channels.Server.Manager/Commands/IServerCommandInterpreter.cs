namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface IServerCommandInterpreter
    {
        ServerCommandType InterpretType(string type);
        ServerCommandTarget InterpretTarget(string handlerId);
    }
}
