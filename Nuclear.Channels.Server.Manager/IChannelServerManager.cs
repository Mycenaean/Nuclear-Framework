using Nuclear.Channels.Server.Manager.Commands;

namespace Nuclear.Channels.Server.Manager
{
    public interface IChannelServerManager
    {
        IChannelServer Server { get; }
        IServerInstructionCollection Instructions { get; }
    }

}