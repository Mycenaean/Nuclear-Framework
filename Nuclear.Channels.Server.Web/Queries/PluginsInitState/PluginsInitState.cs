namespace Nuclear.Channels.Server.Web.Queries.PluginsInitState
{
    public class PluginsInitState
    {
        public bool Success { get; }
        public string Error { get; }

        public PluginsInitState(bool success,string error)
        {
            Success = success;
            Error = error;
        }
    }

}
