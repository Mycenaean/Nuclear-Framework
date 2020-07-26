namespace Nuclear.Channels.Server.Web.Commands.Start
{
    public class StartChannelMethodCommand
    {
        public string HandlerId { get; }

        public StartChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
