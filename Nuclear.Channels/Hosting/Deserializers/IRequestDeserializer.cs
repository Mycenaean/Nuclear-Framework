using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Hosting.Deserializers
{
    internal interface IRequestDeserializer
    {
        List<object> Deserialize(string inputBody, Dictionary<string, Type> methodDescription);
    }
}
