using Nuclear.Channels.Contracts;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using Nuclear.Channels.Interfaces;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
        private readonly IServiceLocator Services;
        public List<ChannelDocument> ChannelDocs;
        public List<ChannelMethodDocument> ChannelMethodDocs;

        public ChannelDocumentationService()
        {
            Services = ServiceLocatorBuilder.CreateServiceLocator();
            ChannelDocs = new List<ChannelDocument>();

            Debug.Assert(Services != null);
        }

        public List<ChannelDocument> GetDocumentation(AppDomain domain)
        {
            Debug.Assert(Services.Get<IChannelLocator>() != null);

            List<Type> channels = Services.Get<IChannelLocator>().RegisteredChannels(domain);
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
                    Dictionary<string, Type> description = Services.Get<IChannelMethodDescriptor>().GetMethodDescription(method);
                    ChannelMethodAttribute ChannelMethodAttribute = method.GetCustomAttribute<ChannelMethodAttribute>();
                    ChannelHttpMethod HttpMethod = ChannelMethodAttribute.HttpMethod;
                    Dictionary<string, Type>.KeyCollection keys = description.Keys;
                    Dictionary<string, Type>.ValueCollection values = description.Values;
                    methodDocument.HttpMethod = HttpMethod;
                    methodDocument.InputParameters = keys != null ? keys.ToArray() : null;
                    methodDocument.InputParameterTypes = values != null ? values.ToArray() : null;
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