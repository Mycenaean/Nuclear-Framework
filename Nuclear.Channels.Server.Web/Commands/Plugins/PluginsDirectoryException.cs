﻿// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Server.Web.Commands.Plugins
{
    public class PluginsDirectoryException : Exception
    {
        public PluginsDirectoryException(string message) : base(message)
        {

        }
    }
}
