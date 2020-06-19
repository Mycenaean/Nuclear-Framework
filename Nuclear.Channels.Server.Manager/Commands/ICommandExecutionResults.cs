namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface ICommandExecutionResults
    {
        void AddResult(CommandId cmdId, object result);
        object GetResult(CommandId cmdId);
        CommandId GetLastCommandId();
    }
}
