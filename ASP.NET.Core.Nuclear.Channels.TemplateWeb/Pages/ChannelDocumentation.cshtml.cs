using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nuclear.Channels.DocumentationTool;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;

namespace ASP.NET.Core.Nuclear.Channels.TemplateWeb.Pages
{
    public class ChannelDocumentationModel : PageModel
    {
        private IServiceLocator Services;
        public List<ChannelDocument> Documents;

        public ChannelDocumentationModel()
        {
            Services = ServiceLocatorBuilder.CreateServiceLocator();
        }
        public void OnGet()
        {
            Documents = Services.Get<IChannelDocumentationService>().GetDocumentation(AppDomain.CurrentDomain);
        }
    }
}