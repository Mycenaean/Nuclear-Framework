namespace Nuclear.Channels.Server.Web.Commands.Restart
{
    public class RestartChannelMethodCommand
    {
        public string HandlerId { get; }

        public RestartChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
