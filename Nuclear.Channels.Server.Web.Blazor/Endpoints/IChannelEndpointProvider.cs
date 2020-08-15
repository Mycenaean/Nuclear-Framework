namespace Nuclear.Channels.Server.Web.Blazor.Endpoints
{
    public interface IChannelEndpointProvider
    {
        string GetEndpointUrl(string baseUrl, string methodName);
    }
}
