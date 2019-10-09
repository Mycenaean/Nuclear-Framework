using System;

namespace Nuclear.Channels
{
    /// <summary>
    /// Service that will host the Channels
    /// </summary>
    public interface IChannelHost
    {
        /// <summary>
        /// Method to load all assemblies containing channels
        /// </summary>
        /// <param name="domain">Current AppComain</param>
        /// <param name="assemblies">Assembly names of your projects</param>
        void LoadAssemblies(AppDomain domain, string[] assemblies);

        /// <summary>
        /// Starts hosting
        /// </summary>
        void StartHosting(string baseURL);
    }
}
