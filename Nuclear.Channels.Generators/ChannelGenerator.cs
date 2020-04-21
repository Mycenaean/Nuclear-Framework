// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Base;
using Nuclear.Channels.Base.Decorators;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Nuclear.Channels.Generators
{
    [Export(typeof(IChannelGenerator), ExportLifetime.Singleton)]
    public class ChannelGenerator : IChannelGenerator
    {
        private static Dictionary<Type, object> _existingChannels;
        private readonly IImportedServicesResolver _importResolver;
        private readonly IServiceLocator _services;
        private readonly IChannelMethodContextProvider _contextProvider;

        public ChannelGenerator()
        {
            _existingChannels = new Dictionary<Type, object>();
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            _importResolver = _services.Get<IImportedServicesResolver>();
            _contextProvider = _services.Get<IChannelMethodContextProvider>();
        }

        public object GetInstance(Type channel)
        {
            if (_existingChannels.ContainsKey(channel))
            {
                MethodInfo initContext = channel.BaseType.GetProperty("Context").GetSetMethod(true);
                IChannelMethodContext context = _contextProvider.GetDefaultContext();
                object[] mparam = new object[] { context };
                initContext.Invoke(_existingChannels[channel], mparam);
                return _existingChannels[channel];
            }

            ConstructorInfo ctor = channel.GetConstructor(new Type[0]);
            string methodName = $"{channel.Name}Ctor";
            DynamicMethod dynamicMtd = new DynamicMethod(methodName, channel, new Type[0]);
            ILGenerator iLGenerator = dynamicMtd.GetILGenerator();
            iLGenerator.Emit(OpCodes.Newobj, ctor);
            iLGenerator.Emit(OpCodes.Ret);

            ChannelObject channelObject = (ChannelObject)dynamicMtd.CreateDelegate(typeof(ChannelObject));
            object channelInstace = channelObject();

            PropertyInfo[] properties = channel.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if(property.GetCustomAttribute<ImportedServiceAttribute>() != null)
                {
                    object service = _importResolver.GetImportedService(property.PropertyType);
                    property.SetValue(channelInstace, service);
                }
            }

            _existingChannels.Add(channel, channelInstace);
            return channelInstace;
        }

        public Dictionary<Type, object> GetKnownChannels()
        {
            return _existingChannels;
        }
    }
}
