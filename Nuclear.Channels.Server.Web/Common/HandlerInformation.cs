using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web.Common
{
    public class HandlerInformation
    {
        public string HandlerId { get; }
        public string Url { get; }
        public string State { get; internal set; }
        public bool Protected { get; }
        public List<string> History { get; set; }

        public HandlerInformation(string handlerId, string url, string state, bool @protected = false)
        {
            HandlerId = handlerId;
            Url = url;
            State = state;
            History = new List<string>();
            Protected = @protected;
        }
    }
}