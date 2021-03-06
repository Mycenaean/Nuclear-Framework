﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.Settings
{
    public interface IWebServerSettings
    {
        IEnumerable<UserSettings> Users { get; }
    }
}
