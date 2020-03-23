// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Newtonsoft.Json.Linq;
using Nuclear.Channels.Base;
using Nuclear.Channels.Data.Deserializers;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Data
{
    /// <summary>
    /// JSON Deserialization Implementation
    /// </summary>
    [Export(typeof(IJsonRequestService), ExportLifetime.Transient)]
    internal class JsonRequestService : IJsonRequestService
    {
        public List<object> Deserialize(string inputBody, ChannelMethodInfo methodDescription)
        {
            List<object> parameters = new List<object>();
            JObject propObject = JObject.Parse(inputBody);

            Debug.Assert(propObject != null);

            foreach (ChannelMethodParameter parameter in methodDescription.Parameters)
            {
                JProperty jProperty = propObject.Property(parameter.Name, StringComparison.OrdinalIgnoreCase);
                if (jProperty != null)
                {
                    if (jProperty.Value != null)
                        parameters.Add(jProperty.Value.ToObject(parameter.Type));
                }
                else
                    continue;
            }
            return parameters;
        }
    }
}
