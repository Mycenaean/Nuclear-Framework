using System.Runtime.CompilerServices;

namespace Nuclear.Channels.Server.Web.Logging
{
    public interface IChannelLogger
    {
        void LogInfo(string message, [CallerMemberName] string method = null);
        void LogDebug(string message, [CallerMemberName] string method = null);
        void LogError(string message, [CallerMemberName] string method = null);

        void LogInfo(object message, [CallerMemberName] string method = null);
        void LogDebug(object message, [CallerMemberName] string method = null);
        void LogError(object message, [CallerMemberName] string method = null);
    }
}
