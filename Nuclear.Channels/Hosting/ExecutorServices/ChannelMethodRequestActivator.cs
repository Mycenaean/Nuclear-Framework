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
        private IServiceLocator Services;
        private IChannelMethodInvoker _channelMethodInvoker;
        private IChannelMessageService _channelMessageService;

        public ChannelMethodRequestActivator()
        {
            Services = ServiceLocatorBuilder.CreateServiceLocator();
            _channelMethodInvoker = Services.Get<IChannelMethodInvoker>();
            _channelMessageService = Services.Get<IChannelMessageService>();

            Debug.Assert(Services != null);
            Debug.Assert(_channelMethodInvoker != null);
            Debug.Assert(_channelMessageService != null);
        }

        public void GetActivateWithoutParameters(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            _channelMethodInvoker.InvokeChannelMethod(channel, method, response);
        }

        public void GetActivateWithParameters(Type channel, MethodInfo method, List<object> channelRequestBody, Dictionary<string, Type> methodDescription, HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                channelRequestBody = new List<object>();
                Dictionary<string, string> deserializedNameValues = new Dictionary<string, string>();
                NameValueCollection urlParameters = request.QueryString;
                for (int i = 0; i < urlParameters.AllKeys.Length; i++)
                {
                    deserializedNameValues.Add(urlParameters.Keys[i].ToLower(), urlParameters[i]);
                }
                foreach (var description in methodDescription)
                {
                    Type paramType = description.Value;
                    try
                    {
                        if (deserializedNameValues.Any(x => x.Key.Equals(description.Key.ToLower())))
                        {
                            KeyValuePair<string, string> exists = deserializedNameValues.FirstOrDefault(x => x.Key.Equals(description.Key.ToLower()));

                            if (paramType == typeof(string))
                                channelRequestBody.Add(exists.Value);
                            else if (paramType == typeof(int))
                                channelRequestBody.Add(int.Parse(exists.Value));
                            else if (paramType == typeof(double))
                                channelRequestBody.Add(double.Parse(exists.Value));
                            else if (paramType == typeof(decimal))
                                channelRequestBody.Add(decimal.Parse(exists.Value));
                            else if (paramType == typeof(float))
                                channelRequestBody.Add(float.Parse(exists.Value));
                            else if (paramType == typeof(bool))
                                channelRequestBody.Add(bool.Parse(exists.Value));
                            else
                                channelRequestBody = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        LogChannel.Write(LogSeverity.Error, ex.Message);
                        StreamWriter writer = new StreamWriter(response.OutputStream);
                        _channelMessageService.ExceptionHandler(writer, ex, response);
                        writer.Flush();
                        writer.Close();
                        response.Close();
                    }
                }
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

        public void PostActivate(Type channel, MethodInfo method, List<object> channelRequestBody, Dictionary<string, Type> methodDescription, HttpListenerRequest request, HttpListenerResponse response)
        {
            string inputRequest = string.Empty;
            using (StreamReader reader = new StreamReader(request.InputStream))
            {
                inputRequest = reader.ReadToEnd();
            }

            LogChannel.Write(LogSeverity.Info, $"Request body: ");
            LogChannel.Write(LogSeverity.Info, inputRequest);

            if (request.ContentType == "application/xml" || request.ContentType == "text/xml; charset=utf-8")
                channelRequestBody = Services.Get<IXmlRequestService>().Deserialize(inputRequest, methodDescription);
            else if (request.ContentType == "application/json" || request.ContentType == "application/json; charset=utf-8")
                channelRequestBody = Services.Get<IJsonRequestService>().Deserialize(inputRequest, methodDescription);
            else
                throw new ChannelMethodContentTypeException("Content-type must be application/json or application/xml");

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
