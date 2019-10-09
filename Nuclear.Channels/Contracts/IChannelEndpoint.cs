namespace Nuclear.Channels.Contracts
{
    /// <summary>
    /// Http Endpoint to be initialized
    /// </summary>
    interface IChannelEndpoint
    {
        string URL { get; set; }
        string Name { get; set; }
    }
}
