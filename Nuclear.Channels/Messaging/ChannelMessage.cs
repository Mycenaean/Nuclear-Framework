using Nuclear.Channels.Messaging.Services.ChannelMessage;

namespace Nuclear.Channels.Messaging
{
    /// <summary>
    /// Base return type of HttpListenerResponse
    /// </summary>
    public class ChannelMessage : IChannelMessage
    {
        public bool Success { get; set; }
        public object Output { get; set; }
        public string Message { get; set; }

    }
}
