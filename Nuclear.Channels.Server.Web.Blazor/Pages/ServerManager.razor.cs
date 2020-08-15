using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Nuclear.Channels.Remoting;
using Nuclear.Channels.Server.Web.Blazor.Endpoints;
using Nuclear.Channels.Server.Web.Blazor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Web.Blazor.Pages
{
    public class ServerManagerBase : ComponentBase
    {
        [Inject]
        public IOptions<ChannelAuthUser> ChannelAuth { get; set; }

        [Inject]
        public IChannelEndpointProvider EndpointProvider { get; set; }

        [Inject]
        public IChannelRemotingClient RemotingClient { get; set; }

        [Inject]
        public RemoteChannelEndpoints Endpoints { get; set; }

        public List<HandlerInfo> Handlers { get; set; }

        public string PluginsState { get; set; }

        private ChannelAuthUser _authUser;

        protected override Task OnInitializedAsync()
        {
            _authUser = ChannelAuth.Value;
            Handlers = new List<HandlerInfo>();
            GetHandlers();
            return base.OnInitializedAsync();
        }

        public void InitChannelPlugins()
        {
            var channelRequest = new ChannelMethodPostRequest();
            channelRequest.Url = EndpointProvider.GetEndpointUrl(Endpoints.PluginsChannel, PluginsChannelEndpoints.InitPluginsEndpoint);
            channelRequest.Parameters.AddParameter("",new object());
            channelRequest.Credentials = new ChannelBasicCredentials() { Username = _authUser.Username, Password = _authUser.Password };

            RemotingClient.Send(channelRequest);
        }

        public void InitChannelsState()
        {
            var channelRequest = new ChannelMethodGetRequest();
            channelRequest.Url = EndpointProvider.GetEndpointUrl(Endpoints.PluginsChannel, PluginsChannelEndpoints.PluginsInitStatus);
            channelRequest.Credentials = new ChannelBasicCredentials() { Username = _authUser.Username, Password = _authUser.Password };
            try
            {
                var response = RemotingClient.GetResponse<PluginsStateInfo>(channelRequest);
                PluginsState = response.Success ? "Initialized" : response.Error;
            }
            catch(Exception ex)
            {
                PluginsState = ex.Message;
            }
        }

        public void GetHandlers()
        {
            var channelRequest = new ChannelMethodGetRequest();
            channelRequest.Url = EndpointProvider.GetEndpointUrl(Endpoints.ServerChannel,ServerChannelEndpoints.GetAllMethods);
            channelRequest.Credentials = new ChannelBasicCredentials() { Username = _authUser.Username, Password = _authUser.Password };
            try
            {
                Handlers = RemotingClient.GetResponse<List<HandlerInfo>>(channelRequest);                
            }
            catch(Exception ex)
            {
                PluginsState = ex.Message;
            }

        }
    }    
}
