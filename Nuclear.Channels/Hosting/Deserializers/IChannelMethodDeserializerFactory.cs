using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Deserializers
{
    internal interface IChannelMethodDeserializerFactory
    {
        List<object> DeserializeFromQueryParameters(Dictionary<string, Type> methodDescription);
        List<object> DeserializeFromBody(Dictionary<string, Type> methodDescription, string contentType);

    }
}
