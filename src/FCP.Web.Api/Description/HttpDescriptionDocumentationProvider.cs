using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace FCP.Web.Api
{
    /// <summary>
    /// Http描述信息提供程序
    /// </summary>
    public class HttpDescriptionDocumentationProvider : IDocumentationProvider
    {
        public string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            return String.Empty;
        }

        public string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            var apiDocumentation = actionDescriptor.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (apiDocumentation != null)
            {
                return apiDocumentation.Description;
            }

            return String.Empty;
        }

        public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
        {
            var apiDocumentation = controllerDescriptor.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (apiDocumentation != null)
            {
                return apiDocumentation.Description;
            }

            return String.Empty;
        }

        public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            return String.Empty;
        }
    }
}
