using Nuclear.Channels.Hosting.Deserializers;
using Nuclear.Data.Xml;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Nuclear.Channels.Hosting
{
    /// <summary>
    /// XML Deserialization Implementation
    /// </summary>
    [Export(typeof(IXmlRequestService),ExportLifetime.Transient)]
    internal class XmlRequestService : IXmlRequestService
    {
        public List<object> Deserialize(string inputBody, Dictionary<string, Type> methodDescription)
        {
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(inputBody);

            Debug.Assert(requestXml.HasChildNodes);
            List<object> channelRequestBody;
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
                        channelRequestBody.Add(XmlTools.DeserializeComplex(inputBody, paramTypeASM, paramTypeFull));
                }
            }
            else
                channelRequestBody = null;

            return channelRequestBody;
        }
    }
}
