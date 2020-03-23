// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using System;
using System.Collections.Generic;

namespace Nuclear.Channels.Data.Deserializers
{
    internal interface IRequestDeserializer
    {
        List<object> Deserialize(string inputBody, ChannelMethodInfo methodDescription);
    }
}
