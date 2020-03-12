// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.ExportLocator;
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

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Hosting.ExecutorServices
{
    /// <summary>
    /// IChannelMethodInvoker Implementation
    /// </summary>
    [Export(typeof(IChannelMethodInvoker), Lifetime = ExportLifetime.Transient)]
    internal class ChannelMethodInvoker : IChannelMethodInvoker
    {
        private IServiceLocator _services;
        private IChannelMessageService _channelMessageService;
        private IChannelMessageOutputWriter _channelMessageWriter;
        private static bool alreadyInvokedFlag = false;

        public ChannelMethodInvoker()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _channelMessageService = _services.Get<IChannelMessageService>();
            _channelMessageWriter = _services.Get<IChannelMessageOutputWriter>();
            Debug.Assert(_services != null);
            Debug.Assert(_channelMessageService != null);
            Debug.Assert(_channelMessageWriter != null);

            _channelMessageWriter.OnPostMessageServiceInvoked += _channelMessageWriter_OnPostMessageServiceInvoked;
        }

        private void _channelMessageWriter_OnPostMessageServiceInvoked(object sender, EventArgs e)
        {
            alreadyInvokedFlag = true;
        }

        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            if (method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
                InvokeChannelMethodAsync(channel, method, response, channelRequestBody);
            else
                InvokeChannelMethodSync(channel, method, response, channelRequestBody);
        }

        public void InvokeChannelMethod(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            if (method.GetCustomAttribute<AsyncStateMachineAttribute>() != null)
                InvokeChannelMethodAsync(channel, method, response, null);
            else
            {
                object chResponse = method.Invoke(Activator.CreateInstance(channel), null);
                WriteResponse(chResponse, response);
            }
        }

        public void InvokeChannelMethodSync(Type channel, MethodInfo method, HttpListenerResponse response, List<object> channelRequestBody)
        {
            object chResponse = method.Invoke(Activator.CreateInstance(channel), channelRequestBody.ToArray());
            WriteResponse(chResponse, response);
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
            WriteResponse(resultValue, response);
        }

        private void WriteResponse(object channelMethodReturnValue, HttpListenerResponse response)
        {
            if (alreadyInvokedFlag)
            {
                alreadyInvokedFlag = false;
                return;
            }

            _channelMessageService.WriteHttpResponse(channelMethodReturnValue, response);
        }
    }
}
