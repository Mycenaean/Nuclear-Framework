// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.InvokerServices.Contracts;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Nuclear.Channels.Base;

namespace Nuclear.Channels.DocumentationTool
{
    /// <summary>
    /// Implementation for IChannelDocumentationService
    /// </summary>
    /// <export>
    /// ExportLifetime is Transient because channels can be imported as a plugins without the need for restart of the application
    /// </export>
    [Export(typeof(IChannelDocumentationService), Lifetime = ExportLifetime.Transient)]
    public class ChannelDocumentationService : IChannelDocumentationService
    {
        private readonly IServiceLocator _services;
        public List<ChannelDocument> ChannelDocs;
        public List<ChannelMethodDocument> ChannelMethodDocs;

        public ChannelDocumentationService()
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            ChannelDocs = new List<ChannelDocument>();

            Debug.Assert(_services != null);
        }

        public List<ChannelDocument> GetDocumentation(AppDomain domain)
        {
            Debug.Assert(_services.Get<IChannelLocator>() != null);

            List<Type> channels = _services.Get<IChannelLocator>().RegisteredChannels(domain);
            foreach (var channel in channels)
            {
                ChannelDocument channelDoc = new ChannelDocument();
                ChannelAttribute channelAttr = channel.GetCustomAttribute<ChannelAttribute>();
                string channelDescription = channelAttr.Description == null ? string.Empty : channelAttr.Description.ToString();
                string channelName = String.IsNullOrEmpty(channelAttr.Name) ? channel.Name : channelAttr.Name;
                string channelRoute = $"~/channels/{channelName}";
                MethodInfo[] methods = channel.GetMethods().Where(x => x.GetCustomAttribute<ChannelMethodAttribute>() != null).ToArray();
                ChannelMethodDocs = new List<ChannelMethodDocument>();
                foreach (var method in methods)
                {

                    ChannelMethodDocument methodDocument = new ChannelMethodDocument();
                    ChannelMethodInfo description = _services.Get<IChannelMethodDescriptor>().GetMethodDescription(method);
                    ChannelMethodAttribute ChannelMethodAttribute = method.GetCustomAttribute<ChannelMethodAttribute>();
                    ChannelHttpMethod HttpMethod = ChannelMethodAttribute.HttpMethod;
                    string[] names = description.Parameters.Select(x => x.Name).ToArray();
                    Type[] types = description.Parameters.Select(x => x.Type).ToArray() ;
                    methodDocument.HttpMethod = HttpMethod;
                    methodDocument.InputParameters = names;
                    methodDocument.InputParameterTypes = types;
                    methodDocument.ReturnTypeName = method.ReturnType.Name;
                    methodDocument.ReturnType = method.ReturnType;
                    methodDocument.URL = $"{channelRoute}/{method.Name}/";
                    methodDocument.AuthSchema = ChannelMethodAttribute.Schema;
                    methodDocument.Description = ChannelMethodAttribute.Description; //null possibility

                    ChannelMethodDocs.Add(methodDocument);
                }
                channelDoc.Name = channel.Name;
                channelDoc.Description = channelDescription; //null possibility
                channelDoc.URL = channelRoute;
                channelDoc.AvailableEndpoints = ChannelMethodDocs;

                ChannelDocs.Add(channelDoc);
            }


            return ChannelDocs;
        }
    }
}