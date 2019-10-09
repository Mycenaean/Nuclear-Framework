using Nuclear.ExportLocator.Services;
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
        void Execute(AppDomain currentDomain, IServiceLocator Services, string baseURL = null);

        /// <summary>
        /// Set authentication options
        /// </summary>
        /// <param name="authMethodClass">Class in which Authentication Method is defined , method must take IPrincipal as a parameter and must be of boolean return type</param>
        /// <param name="authMethod">User defined Authentication Method Info</param>
        void AuthenticationOptions(Type authMethodClass, MethodInfo authMethod);
    }
}
