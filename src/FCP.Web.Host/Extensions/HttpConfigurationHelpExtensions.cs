using FCP.Util;
using FCP.Web.Api;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.HelpPage;

namespace FCP.Web.Host
{
    public static class HttpConfigurationHelpExtensions
    {
        public static HttpConfiguration UseApiHelp(this HttpConfiguration configuration, string helpControllerName)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (helpControllerName.isNullOrEmpty())
                throw new ArgumentNullException(nameof(helpControllerName));

            configuration.Routes.MapHttpRoute(
                name: FCPApiHelpController.routeName,
                routeTemplate: string.Format("api/{0}/{1}id{2}", FCPApiHelpController.controllerRoute, "{", "}"),
                defaults: new { controller = helpControllerName, id = RouteParameter.Optional }
            );

            configuration.SetDocumentationProvider(new WebApiDescriptionDocumentationProvider());
            configuration.Services.Replace(typeof(IApiExplorer), new FCPDoResultApiExplorer(configuration));

            return configuration;
        }
    }
}
