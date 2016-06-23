using System.Web.Http.Controllers;
using System.Web.Http.Description;
using FCP.Util;

namespace FCP.Web.Api
{
    /// <summary>
    /// Http描述符 扩展
    /// </summary>
    public static class HttpDescriptorExtension
    {
        private static IDocumentationProvider documentationProvider = new HttpDescriptionDocumentationProvider();

        /// <summary>
        /// 获取action的 描述信息
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static string description(this HttpActionDescriptor actionDescriptor)
        {
            var description = documentationProvider.GetDocumentation(actionDescriptor);

            if (description.isNullOrEmpty())
            {
                description = actionDescriptor.ActionName;
            }
            return description;
        }

        /// <summary>
        /// 获取controller的 描述信息
        /// </summary>
        /// <param name="controllerDescriptor"></param>
        /// <returns></returns>
        public static string description(this HttpControllerDescriptor controllerDescriptor)
        {
            var description = documentationProvider.GetDocumentation(controllerDescriptor);

            if (description.isNullOrEmpty())
            {
                description = controllerDescriptor.ControllerName;
            }
            return description;
        }
    }
}
