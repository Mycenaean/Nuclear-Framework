namespace Nuclear.Channels.Messaging.Services.ChannelMessage
{
    /// <summary>
    /// Base return type of HttpListenerResponse
    /// </summary>
    public interface IChannelMessage
    {
        bool Success { get; set; }
        object Output { get; set; }
        string Message { get; set; }

    }
}
