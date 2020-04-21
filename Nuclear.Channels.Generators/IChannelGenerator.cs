// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Generators
{
    /// <summary>
    /// Service responsible for creation of Channel instances
    /// </summary>
    public interface IChannelGenerator
    {
        object GetInstance(Type channel);
        Dictionary<Type, object> GetKnownChannels();
    }
}
