// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nuclear.ExportLocator.Global
{
    /// <summary>
    /// Class that will get all services decorated with the ExportAttribute
    /// </summary>
    internal class ExportFactory
    {
        public IList<ExportInformation> exports = new List<ExportInformation>();

        /// <summary>
        /// Method that gets all exported Services
        /// </summary>
        /// <returns>List of exported services enclosed in ExportInformation</returns>
        public IList<ExportInformation> GetExports()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var export in from assembly in assemblies
                                   from type in assembly.GetTypes()
                                   from Attribute attr in type.GetCustomAttributes()
                                   where attr is ExportAttribute
                                   let newAttr = attr as ExportAttribute
                                   let export = new ExportInformation
                                   {
                                       ServiceType = newAttr.ServiceType,
                                       Implementation = type,
                                       ExportLifetime = newAttr.Lifetime

                                   }
                                   select export)
            {
                exports.Add(export);
            }

            return exports;
        }


    }
}
