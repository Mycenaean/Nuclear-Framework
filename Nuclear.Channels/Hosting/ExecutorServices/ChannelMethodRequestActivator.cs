// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Hosting.Deserializers;
using Nuclear.Channels.Hosting.Exceptions;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
using Nuclear.Data.Xml;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Hosting.ExecutorServices
{
    [Export(typeof(IChannelMethodRequestActivator), Lifetime = ExportLifetime.Scoped)]
    internal class ChannelMethodRequestActivator : IChannelMethodRequestActivator
    {
        private IServiceLocator _services;
        private IChannelMethodInvoker _channelMethodInvoker;
        private IChannelMessageService _channelMessageService;

        public ChannelMethodRequestActivator()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _channelMethodInvoker = _services.Get<IChannelMethodInvoker>();
            _channelMessageService = _services.Get<IChannelMessageService>();

            Debug.Assert(_services != null);
            Debug.Assert(_channelMethodInvoker != null);
            Debug.Assert(_channelMessageService != null);
        }

        public void GetActivateWithoutParameters(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            _channelMethodInvoker.InvokeChannelMethod(channel, method, response);
        }

        public void GetActivateWithParameters(Type channel, MethodInfo method, List<object> channelRequestBody, HttpListenerResponse response)
        {
            try
            {
                _channelMethodInvoker.InvokeChannelMethod(channel, method, response, channelRequestBody);
            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(response.OutputStream);
                _channelMessageService.ExceptionHandler(writer, ex, response);
                writer.Flush();
                writer.Close();
            }
        }

        public void PostActivate(Type channel, MethodInfo method, List<object> channelRequestBody, HttpListenerResponse response)
        {
            if (channelRequestBody == null)
                throw new ChannelMethodParameterException("Parameters do not match");

            try
            {
                _channelMethodInvoker.InvokeChannelMethod(channel, method, response, channelRequestBody);
            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(response.OutputStream);
                _channelMessageService.ExceptionHandler(writer, ex, response);
                writer.Flush();
                writer.Close();
            }
        }
    }
}
