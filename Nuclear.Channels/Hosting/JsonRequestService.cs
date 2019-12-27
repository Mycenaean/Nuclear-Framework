// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Newtonsoft.Json.Linq;
using Nuclear.Channels.Hosting.Deserializers;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Hosting
{
    /// <summary>
    /// JSON Deserialization Implementation
    /// </summary>
    [Export(typeof(IJsonRequestService), ExportLifetime.Transient)]
    internal class JsonRequestService : IJsonRequestService
    {
        public List<object> Deserialize(string inputBody, Dictionary<string, Type> methodDescription)
        {
            List<object> parameters = new List<object>();
            JObject propObject = JObject.Parse(inputBody);

            Debug.Assert(propObject != null);

            foreach (var property in methodDescription)
            {
                JProperty jProperty = propObject.Property(property.Key, StringComparison.OrdinalIgnoreCase);
                if (jProperty != null)
                {
                    if (jProperty.Value != null)
                        parameters.Add(jProperty.Value.ToObject(property.Value));
                }
                else
                    continue;
            }
            return parameters;
        }
    }
}
