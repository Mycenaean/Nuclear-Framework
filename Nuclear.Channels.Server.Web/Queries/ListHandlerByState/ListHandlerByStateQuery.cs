namespace Nuclear.Channels.Server.Web.Queries.ListHandlerByState
{
    public class ListHandlerByStateQuery
    {
        public string State { get; }

        public ListHandlerByStateQuery(string state)
        {
            State = state;
        }
    }

}
