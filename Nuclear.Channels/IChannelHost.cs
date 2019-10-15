using Nuclear.Channels.Hosting.Exceptions;
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
        /// AuthenticationOptions for Token Authentication
        /// </summary>
        /// <param name="tokenAuthenticationMethod">Delegate for basic authentication</param>
        void AuthenticationOptions(Func<string, bool> tokenAuthenticationMethod);

        /// <summary>
        /// AuthenticationOptions for Basic Authentication
        /// </summary>
        /// <param name="tokenAuthenticationMethod">Delegate for token authentication</param>
        void AuthenticationOptions(Func<string, string, bool> basicAuthenticationMethod);

        /// <summary>
        /// Starts hosting
        /// </summary>
        /// <exception cref="HttpListenerNotSupportedException"></exception>
        void StartHosting(string baseURL);
    }
}
