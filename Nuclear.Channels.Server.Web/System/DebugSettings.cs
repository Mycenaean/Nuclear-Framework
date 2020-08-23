// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.IO;
using System.Reflection;

namespace Nuclear.Channels.Server.Web.System
{
    public class DebugSettings : ISystemSettings
    {
        public string BaseDirectory => (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;
    }
}
