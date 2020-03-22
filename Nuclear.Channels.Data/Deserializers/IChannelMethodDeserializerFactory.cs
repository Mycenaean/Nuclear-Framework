// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;

namespace Nuclear.Channels.Data.Deserializers
{
    internal interface IChannelMethodDeserializerFactory
    {
        List<object> DeserializeFromQueryParameters(Dictionary<string, Type> methodDescription);
        List<object> DeserializeFromBody(Dictionary<string, Type> methodDescription, string contentType);

    }
}
