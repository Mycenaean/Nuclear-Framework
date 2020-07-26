namespace Nuclear.Channels.Server.Web.Queries.HandlerHistory
{
    public class HandlerHistoryQuery
    {
        public string HandlerId { get; set; }

        public HandlerHistoryQuery(string handlerId)
        {
            HandlerId = handlerId;
        }
    }

}
