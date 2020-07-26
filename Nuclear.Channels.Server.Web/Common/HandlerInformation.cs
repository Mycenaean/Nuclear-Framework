namespace Nuclear.Channels.Server.Web.Common
{
    public class HandlerInformation
    {
        public string HandlerId { get; }
        public string Url { get; }
        public string State { get; internal set; }

        public HandlerInformation(string handlerId, string url, string state)
        {
            HandlerId = handlerId;
            Url = url;
            State = state;
        }
    }
}