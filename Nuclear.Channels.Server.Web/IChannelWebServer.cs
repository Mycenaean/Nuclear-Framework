namespace Nuclear.Channels.Server.Web
{
    public interface IChannelWebServer
    {
        IChannelServer Server { get; set; }
        IChannelServer ServerCopy { get; }
        void Start();
    }
}