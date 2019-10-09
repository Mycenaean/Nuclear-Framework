namespace Nuclear.Channels.Contracts
{
    /// <summary>
    /// Http Endpoint to be initialized
    /// </summary>
    public class ChannelEndpoint : IChannelEndpoint
    {
        public string URL { get; set; }
        public string Name { get; set; }

    }
}
