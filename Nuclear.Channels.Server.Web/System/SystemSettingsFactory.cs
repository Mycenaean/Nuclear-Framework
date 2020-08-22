// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.System
{
    public class SystemSettingsFactory
    {
        public static ISystemSettings GetSettings()
        {
            #if DEBUG
                        return new DebugSettings();
            #else
                        return new ReleaseSettings();
            #endif
        }
    }
}
