using Nuclear.ExportLocator.Services;

namespace Nuclear.Channels.Contracts
{
    /// <summary>
    /// Base contract for abstract ChannelBase class
    /// </summary>
    public interface IChannel
    {
        IServiceLocator Services { get; }
    }
}

