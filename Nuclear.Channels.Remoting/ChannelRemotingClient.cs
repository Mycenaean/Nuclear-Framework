using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuclear.Channels.Remoting.Enums;
using Nuclear.Channels.Remoting.Exceptions;
using Nuclear.Channels.Remoting.Services;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Diagnostics;

namespace Nuclear.Channels.Remoting
{
    [Export(typeof(IChannelRemotingClient), ExportLifetime.Transient)]
    public class ChannelRemotingClient : IChannelRemotingClient
    {
        private IServiceLocator Services;
        private readonly IRemoteInvoker _remoteInvoker;

        [DebuggerStepThrough]
        public ChannelRemotingClient()
        {
            Services = ServiceLocatorBuilder.CreateServiceLocator();
            _remoteInvoker = Services.Get<IRemoteInvoker>();
        }

        public T InvokeChannel<T>(string URL, HttpChannelMethod method, IChannelCredentials credentials)
        {
            string channelResponse = _remoteInvoker.InvokeChannel(URL, method, credentials);
            return (T)DeserializeChannelResponse<T>(channelResponse);
        }

        public T InvokeChannel<T>(string URL, object inputBody, HttpChannelMethod method, IChannelCredentials credentials, string contentType = null)
        {
            string jsonBody = JsonConvert.SerializeObject(inputBody);
            string channelResponse = _remoteInvoker.InvokeChannel(URL, jsonBody, method, credentials, contentType);
            return (T)DeserializeChannelResponse<T>(channelResponse);
        }

        private object DeserializeChannelResponse<T>(string channelResponse)
        {
            JObject responseObject = JObject.Parse(channelResponse);
            string success = responseObject.Property("Success", StringComparison.OrdinalIgnoreCase).Value.ToString();
            if (success == "True")
            {
                string output = responseObject.Property("Output").Value.ToString();
                if (typeof(T) == typeof(string))
                    return output;
                else if (typeof(T) == typeof(int))
                    return int.Parse(output);
                else if (typeof(T) == typeof(bool))
                    return Convert.ToBoolean(output);
                else if (typeof(T) == typeof(double))
                    return double.Parse(output);
                else if (typeof(T) == typeof(decimal))
                    return decimal.Parse(output);
                else if (typeof(T) == typeof(char))
                    return char.Parse(output);
                else
                    return JsonConvert.DeserializeObject(output);
            }
            else
            {
                string message = responseObject.Property("Message", StringComparison.OrdinalIgnoreCase).Value.ToString();
                throw new ChannelRemotingException(message);
            }
        }
    }

}

