// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using Nuclear.Channels.Data.Deserializers;
using Nuclear.Channels.Data.Xml;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Data
{
    /// <summary>
    /// XML Deserialization Implementation
    /// </summary>
    [Export(typeof(IXmlRequestService), ExportLifetime.Transient)]
    internal class XmlRequestService : IXmlRequestService
    {
        public List<object> Deserialize(string inputBody, ChannelMethodInfo methodDescription)
        {
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(inputBody);

            Debug.Assert(requestXml.HasChildNodes);
            List<object> channelRequestBody;
            if (methodDescription.Parameters.Count != 0)
            {
                channelRequestBody = new List<object>();

                //Parameter initialization logic
                foreach (ChannelMethodParameter parameter in methodDescription.Parameters)
                {
                    Type paramType = parameter.Type; // Parameter type
                    string paramTypeFull = paramType.FullName; // Name of the class
                    string paramTypeASM = paramType.Assembly.FullName; // Name of the assembly

                    if (paramType == typeof(string))
                        channelRequestBody.Add(XmlTools.DeserializeString(requestXml, parameter.Name));
                    else if (paramType == typeof(int))
                        channelRequestBody.Add(XmlTools.DeserializeInt(requestXml, parameter.Name));
                    else if (paramType == typeof(double))
                        channelRequestBody.Add(XmlTools.DeserializeDouble(requestXml, parameter.Name));
                    else if (paramType == typeof(decimal))
                        channelRequestBody.Add(XmlTools.DeserializeDecimal(requestXml, parameter.Name));
                    else if (paramType == typeof(float))
                        channelRequestBody.Add(XmlTools.DeserializeFloat(requestXml, parameter.Name));
                    else if (paramType == typeof(bool))
                        channelRequestBody.Add(XmlTools.DeserializeBool(requestXml, parameter.Name));
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
