namespace Nuclear.Channels.Server.Web.Commands.Stop
{
    public class StopChannelMethodCommand
    {
        public string HandlerId { get; set; }

        public StopChannelMethodCommand(string handlerId)
        {
            HandlerId = handlerId;
        }
    }
}
