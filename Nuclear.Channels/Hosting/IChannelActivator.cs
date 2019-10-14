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
        /// <param name="authMethodClass">Class in which Authentication Method is defined , method must take IPrincipal as a parameter and must be of boolean return type</param>
        /// <param name="authMethod">User defined Authentication Method Info</param>
        [Obsolete("Use second overloaded version")]
        void AuthenticationOptions(Type authMethodClass, MethodInfo authMethod);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="authMethod">Function delegate to be used for authentication</param>
        void AuthenticationOptions(Func<string, string, bool> authMethod);
    }
}
