using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
using Nuclear.Data.Xml;
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
using System.Xml;

namespace Nuclear.Channels.Hosting.ExecutorServices
{
    [Export(typeof(IChannelMethodRequestActivator), Lifetime = ExportLifetime.Scoped)]
    public class ChannelMethodRequestActivator : IChannelMethodRequestActivator
    {
        private IServiceLocator Services;
        private IChannelMethodInvoker _channelMethodInvoker;
        private IChannelMessageService _channelMessageService;

        public ChannelMethodRequestActivator()
        {
            Services = ServiceLocator.GetInstance;
            _channelMethodInvoker = Services.Get<IChannelMethodInvoker>();
            _channelMessageService = Services.Get<IChannelMessageService>();

            Debug.Assert(Services != null);
            Debug.Assert(_channelMethodInvoker != null);
            Debug.Assert(_channelMessageService != null);
        }

        /// <summary>
        /// Activate the ChannelMethod which takes no input parameters
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
        public void GetActivateWithoutParameters(Type channel, MethodInfo method, HttpListenerResponse response)
        {
            _channelMethodInvoker.InvokeChannelMethod(channel, method, response);
        }

        /// <summary>
        ///  Activate the ChannelMethod which takes input parameters based on query string 
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="channelRequestBody">List of parameters to be initialized</param>
        /// <param name="methodDescription">List of parameter names with their types</param>
        /// <param name="request">Incoming HttpListenerRequest</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
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
                //Invoke ChannelMethod
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

        /// <summary>
        /// Activate the ChannelMethod which takes input parameters based on post input body
        /// </summary>
        /// <param name="channel">Targeted Channel</param>
        /// <param name="method">Targeted ChannelMethod</param>
        /// <param name="channelRequestBody">List of parameters to be initialized</param>
        /// <param name="methodDescription">List of parameter names with their types</param>
        /// <param name="request">Incoming HttpListenerRequest</param>
        /// <param name="response">HttpListenerResponse to be written to the client</param>
        public void PostActivate(Type channel, MethodInfo method, List<object> channelRequestBody, Dictionary<string, Type> methodDescription, HttpListenerRequest request, HttpListenerResponse response)
        {
            string inputRequest = string.Empty;
            using (StreamReader reader = new StreamReader(request.InputStream))
            {
                inputRequest = reader.ReadToEnd();
            }

            LogChannel.Write(LogSeverity.Info, $"Request body: ");
            LogChannel.Write(LogSeverity.Info, inputRequest);

            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(inputRequest);

            Debug.Assert(requestXml.HasChildNodes);

            if (methodDescription.Values.Count != 0)
            {
                channelRequestBody = new List<object>();

                //Parameter initialization logic
                foreach (var description in methodDescription)
                {
                    Type paramType = description.Value; // Parameter type
                    string paramTypeFull = paramType.FullName; // Name of the class
                    string paramTypeASM = paramType.Assembly.FullName; // Name of the assembly

                    if (paramType == typeof(string))
                        channelRequestBody.Add(XmlTools.DeserializeString(requestXml, description.Key));
                    else if (paramType == typeof(int))
                        channelRequestBody.Add(XmlTools.DeserializeInt(requestXml, description.Key));
                    else if (paramType == typeof(double))
                        channelRequestBody.Add(XmlTools.DeserializeDouble(requestXml, description.Key));
                    else if (paramType == typeof(decimal))
                        channelRequestBody.Add(XmlTools.DeserializeDecimal(requestXml, description.Key));
                    else if (paramType == typeof(float))
                        channelRequestBody.Add(XmlTools.DeserializeFloat(requestXml, description.Key));
                    else if (paramType == typeof(bool))
                        channelRequestBody.Add(XmlTools.DeserializeBool(requestXml, description.Key));
                    else
                        channelRequestBody.Add(XmlTools.DeserializeComplex(inputRequest, paramTypeASM, paramTypeFull));
                }
            }
            else
                channelRequestBody = null;

            //Most probably something went wrong , prepare for Exception strike
            if (channelRequestBody == null)
            {
                if (method.IsStatic)
                    _channelMethodInvoker.InvokeChannelMethod(channel, method, response);
                else
                {
                    _channelMethodInvoker.InvokeChannelMethod(channel, method, response);
                }
            }
            else
            {
                try
                {
                    //Invoke ChannelMethod
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
}
