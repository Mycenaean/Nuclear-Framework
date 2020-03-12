using Nuclear.Channels.Hosting.Contracts;
using Nuclear.Channels.Hosting.Exceptions;
using Nuclear.Data.Logging.Enums;
using Nuclear.Data.Logging.Services;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Hosting.Deserializers
{

    internal class ChannelMethodDeserializerFactory
    {
        private readonly IServiceLocator _services;
        private readonly Stream _requestStream;
        private readonly NameValueCollection _queryParameters;
        public ChannelMethodDeserializerFactory(Stream requestStream)
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _requestStream = requestStream;
        }

        public ChannelMethodDeserializerFactory(NameValueCollection queryParameters)
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _queryParameters = queryParameters;
        }

        public List<object> DeserializeFromBody(Dictionary<string, Type> methodDescription, string contentType)
        {
            string inputRequest = string.Empty;
            using (StreamReader reader = new StreamReader(_requestStream))
            {
                inputRequest = reader.ReadToEnd();
            }

            LogChannel.Write(LogSeverity.Info, $"Request body: ");
            LogChannel.Write(LogSeverity.Info, inputRequest);

            if (contentType == "application/xml" || contentType == "text/xml; charset=utf-8")
                return _services.Get<IXmlRequestService>().Deserialize(inputRequest, methodDescription);
            else if (contentType == "application/json" || contentType == "application/json; charset=utf-8")
                return _services.Get<IJsonRequestService>().Deserialize(inputRequest, methodDescription);
            else
                throw new ChannelMethodContentTypeException("Content-type must be application/json or application/xml");

        }

        public List<object> DeserializeFromQueryParameters(Dictionary<string, Type> methodDescription)
        {
            List<object> channelRequestBody = new List<object>();
            Dictionary<string, string> deserializedNameValues = new Dictionary<string, string>();
            NameValueCollection urlParameters = _queryParameters;
            for (int i = 0; i < urlParameters.AllKeys.Length; i++)
            {
                deserializedNameValues.Add(urlParameters.Keys[i].ToLower(), urlParameters[i]);

            }
            foreach (var description in methodDescription)
            {
                Type paramType = description.Value;
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

            return channelRequestBody;
        }
    }
}
