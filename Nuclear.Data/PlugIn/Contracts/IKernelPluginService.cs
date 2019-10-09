using System;
using System.Collections.Generic;

namespace Nuclear.Data.PlugIn.Contracts
{
    /// <summary>
    /// Contract for service that will get all plugin channels and load assemblies to the host
    /// </summary>
    public interface IKernelPluginService
    {
        /// <summary>
        /// Method that will return additional channels
        /// </summary>
        /// <returns>List of channels</returns>
        [Obsolete("Dont use this method since it is not required. Use use LoadPlugIns")]
        List<Type> GetPlugins(AppDomain domain);

        /// <summary>
        /// Loads assemblies from the PlugIns folder
        /// </summary>
        /// <param name="domain">Application domain</param>
        void LoadPlugIns(AppDomain domain);
    }
}
