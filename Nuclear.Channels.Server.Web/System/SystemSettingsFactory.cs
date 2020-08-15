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
