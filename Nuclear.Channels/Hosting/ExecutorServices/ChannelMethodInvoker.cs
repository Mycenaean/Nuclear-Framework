using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.Output;
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
    /// <summary>
    /// IChannelMethodInvoker Implementation
    /// </summary>
    [Export(typeof(IChannelMethodInvoker), Lifetime = ExportLifetime.Transient)]
    internal class ChannelMethodInvoker : IChannelMethodInvoker
    {
        private IServiceLocator Services;
        private IChannelMessageService _channelMessageService;
        private IChannelMessageWriter _channelMessageWriter;
        private HttpListenerResponse _response;

        public ChannelMethodInvoker()
        {
            Services = ServiceLocator.GetInstance;
            _channelMessageService = Services.Get<IChannelMessageService>();
            _channelMessageWriter = new ChannelMessageWriter();

            Debug.Assert(Services != null);
            Debug.Assert(_channelMessageService != null);
            Debug.Assert(_channelMessageWriter != null);

            _channelMessageWriter.SendChannelMessage += _channelMessageWriter_SendChannelMessage;
        }

        private void _channelMessageWriter_SendChannelMessage(object sender, Messaging.ChannelMethodEventArgs e)
        {
            Debug.Assert(_response!=null);
            _channelMessageService.WriteHttpResponse(e.ChannelMessage, _response);
            _response = null;
        }


        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            _response = response;
            if (method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                InvokeChannelMethodAsync(channel, method, response, channelRequestBody);
            else
                InvokeChannelMethodSync(channel, method, response, channelRequestBody);

            _response = null;
        }

        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            _response = response;
            if (method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                InvokeChannelMethodAsync(channel, method, response, null);
            else
            {
                object chResponse = method.Invoke(Activator.CreateInstance(channel), null);
                _channelMessageService.WriteHttpResponse(chResponse, response);
            }
            _response = null;
        }

        public void InvokeChannelMethodSync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            object chResponse = method.Invoke(Activator.CreateInstance(channel), channelRequestBody.ToArray());
            _channelMessageService.WriteHttpResponse(chResponse,response);
        }

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
            _channelMessageService.WriteHttpResponse(resultValue, response);
        }
    }
}
