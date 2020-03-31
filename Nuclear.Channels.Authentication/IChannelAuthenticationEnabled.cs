using System;
// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuclear.Channels.Authentication
{
    public interface IChannelAuthenticationEnabled
    {
        AuthenticationSettings AuthenticationSettings { get; set; }
    }
}
