using Nuclear.ExportLocator.Services;
using Nuclear.Channels.Hosting.Exceptions;
using System;
using System.Reflection;

namespace Nuclear.Channels.Hosting
{
    /// <summary>
    /// Service that will initialize all ChannelMethods as HTTP Endopints
    /// </summary>
    public interface IChannelActivator
    {
        /// <summary>
        /// Method that will do the initialization of Channels
        /// </summary>
        /// <param name="domain">AppDomain with all assemblies</param>
        /// <param name="Services">IServiceLocator</param>
        /// <exception cref="HttpListenerNotSupportedException"></exception>
        void Execute(AppDomain currentDomain, IServiceLocator Services, string baseURL = null);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="basicAuthenticationMethod">Function delegate to be used for basic authentication</param>
        void AuthenticationOptions(Func<string, string, bool> basicAuthenticationMethod);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="tokenAuthenticationMethod">Function delegate to be used for token authentication</param>
        void AuthenticationOptions(Func<string, bool> tokenAuthenticationMethod);
    }
}
