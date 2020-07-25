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
        private readonly string[] _assemblyNames;
        private readonly IList<ExportInformation> _exports = new List<ExportInformation>();

        public ExportFactory()
        {
                
        }

        public ExportFactory(string[] assemblyNames)
        {
            _assemblyNames = assemblyNames;
        }
        
        /// <summary>
        /// Method that gets all exported Services
        /// </summary>
        /// <returns>List of exported services enclosed in ExportInformation</returns>
        public IList<ExportInformation> GetExports()
        {
            Assembly[] assemblies;
            
            if(_assemblyNames == null || _assemblyNames.Length == 0)
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            else
            {
                assemblies = new Assembly[_assemblyNames.Length];
                
                for (var i = 0; i < _assemblyNames.Length; i++)
                    assemblies[i] = Assembly.Load(_assemblyNames[i]);
            }

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
                _exports.Add(export);
            }

            return _exports;
        }


    }
}
