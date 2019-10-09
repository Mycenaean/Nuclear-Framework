using Nuclear.Channels.Hosting.Contracts;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nuclear.Channels.Hosting.ExecutorServices
{
    [Export(typeof(IChannelMethodInvoker), Lifetime = ExportLifetime.Transient)]
    public class ChannelMethodInvoker : IChannelMethodInvoker
    {
        private IServiceLocator Services;
        private IChannelMessageService _channelMessageService;

        public ChannelMethodInvoker()
        {
            Services = ServiceLocator.GetInstance;
            _channelMessageService = Services.Get<IChannelMessageService>();

            Debug.Assert(Services != null);
            Debug.Assert(_channelMessageService != null);
        }


        /// <summary>
        /// Method that will Invoke targeted ChannelMethod
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            if (method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                InvokeChannelMethodAsync(channel, method, response, channelRequestBody);
            else
                InvokeChannelMethodSync(channel, method, response, channelRequestBody);
        }

        /// <summary>
        /// Method that will Invoke targeted ChannelMethod without parameters
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            if (method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                InvokeChannelMethodAsync(channel, method, response, null);
            else
                _channelMessageService.WriteHttpResponse(response, channel, method);
        }

        /// <summary>
        /// Method that will invoke Sync ChannelMethods
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        public void InvokeChannelMethodSync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            object chResponse = method.Invoke(Activator.CreateInstance(channel), channelRequestBody.ToArray());
            _channelMessageService.WriteHttpResponse(response, channel, method, chResponse);
        }

        /// <summary>
        /// Method that will invoke Async ChannelMethods
        /// </summary>
        /// <param name="channel">Channel instance</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">Response for the client</param>
        /// <param name="channelRequestBody">Parameters</param>
        public void InvokeChannelMethodAsync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            //checking for the input body
            object[] bodyArray;
            if (channelRequestBody != null)
                bodyArray = channelRequestBody.ToArray();
            else
                bodyArray = null;

            Task<Task<object>> task = Task.Factory.StartNew(async () => await Task.Run(() => method.Invoke(Activator.CreateInstance(channel), bodyArray)));
            task.Wait();

            Task<object> chResponseTask = task.Result;
            chResponseTask.Wait();
            Debug.Assert(chResponseTask != null);

            object chResponse = chResponseTask.Result;
            Debug.Assert(chResponse != null);

            PropertyInfo[] properties = chResponse.GetType().GetProperties();
            var result = properties.FirstOrDefault(x => x.Name.Equals("result", StringComparison.OrdinalIgnoreCase));
            object resultValue = result.GetValue(chResponse);
            Debug.Assert(resultValue != null);
            _channelMessageService.WriteHttpResponse(response, channel, method, resultValue);
        }
    }
}
