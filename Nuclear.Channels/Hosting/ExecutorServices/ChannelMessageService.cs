using Newtonsoft.Json;
using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Messaging;
using Nuclear.Channels.Messaging.Services.ChannelMessage;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;

namespace Nuclear.Channels.Hosting.ExecutorServices
{
    [Export(typeof(IChannelMessageService), Lifetime = ExportLifetime.Transient)]
    public class ChannelMessageService : IChannelMessageService
    {
        /// <summary>
        /// Method that will write response to the client
        /// </summary>
        /// <param name="response">HttpListenerResponse instance</param>
        /// <param name="channel">Channel</param>
        /// <param name="method">ChannelMethod</param>
        public void WriteHttpResponse(HttpListenerResponse response, Type channel, MethodInfo method, object chResponse = null)
        {
            response.ContentType = "application/json";
            Stream stream = response.OutputStream;
            using (StreamWriter writer = new StreamWriter(stream))
            {
                try
                {
                    //ChannelMessage to be sent back to the client
                    IChannelMessage respChMessage;
                    object channelmethodResponse;
                    if (chResponse == null)
                        channelmethodResponse = method.Invoke(Activator.CreateInstance(channel), null);
                    else
                        channelmethodResponse = chResponse;

                    Debug.Assert(channelmethodResponse != null);

                    //Response type checking
                    if (channelmethodResponse.GetType() != typeof(IChannelMessage) && channelmethodResponse.GetType() != typeof(ChannelMessage))
                    {
                        respChMessage = new ChannelMessage();
                        respChMessage.Success = true;
                        respChMessage.Output = channelmethodResponse;
                    }
                    else
                        respChMessage = null;

                    //Serialization Logic
                    string outputMsg = string.Empty;
                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                    jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    if (respChMessage == null)
                        outputMsg = JsonConvert.SerializeObject(channelmethodResponse, Formatting.Indented, jsonSettings);
                    else
                        outputMsg = JsonConvert.SerializeObject(respChMessage, Formatting.Indented, jsonSettings);

                    writer.WriteLine(outputMsg);
                    //return;
                }
                catch (Exception ex)
                {
                    ExceptionHandler(writer, ex, response);
                }
            }
        }

        /// <summary>
        /// Method that will proccess the Exception
        /// </summary>
        /// <param name="writer">StreamWriter instance</param>
        /// <param name="ex">Thrown Exception</param>
        /// <param name="response">HttpListenerResponse instance that will be sent to the client</param>
        public void ExceptionHandler(StreamWriter writer, Exception ex, HttpListenerResponse response)
        {
            LogChannel.Write(LogSeverity.Error, "Exception handler called..");
            ChannelMessage errorChMessage = new ChannelMessage()
            {
                Message = ex.Message
            };
            LogChannel.Write(LogSeverity.Error, ex.Message);
            LogChannel.Write(LogSeverity.Error, ex.InnerException == null ? "No Inner Exception" : ex.InnerException.ToString());
            response.ContentType = "application/json";
            string outputMsg = JsonConvert.SerializeObject(errorChMessage, Formatting.Indented);
            writer.Write(outputMsg);
            //return;
        }

        /// <summary>
        /// Failed Auth ChannelMessage
        /// </summary>
        /// <param name="ChannelSchema">AuthenticationSchemes schema</param>
        /// <param name="response">Response for the client</param>
        public void FailedAuthenticationResponse(AuthenticationSchemes ChannelSchema, HttpListenerResponse response)
        {
            LogChannel.Write(LogSeverity.Info, "Authentication failed...Exiting");
            IChannelMessage msg = new ChannelMessage()
            {
                Success = false,
                Message = $"You need to provide {ChannelSchema.ToString()} authentication"
            };
            string outputString = JsonConvert.SerializeObject(msg, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(response.OutputStream))
            {
                writer.WriteLine(outputString);
            }
        }
    }
}
