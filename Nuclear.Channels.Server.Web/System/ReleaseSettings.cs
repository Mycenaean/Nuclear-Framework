// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Server.Web.System
{
    public class ReleaseSettings : ISystemSettings
    {
        public string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }
}
