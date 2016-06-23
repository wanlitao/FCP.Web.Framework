using System;
using System.Reflection;
using FCP.Web.Api;
using WebApi.HelpPage.ModelDescriptions;
using System.ComponentModel;
using System.Linq;

namespace FCP.Web.Host
{
    /// <summary>
    /// WebApi描述信息提供程序
    /// </summary>
    public class WebApiDescriptionDocumentationProvider : HttpDescriptionDocumentationProvider, IModelDocumentationProvider
    {
        public string GetDocumentation(Type type)
        {
            var typeDocumentation = type.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (typeDocumentation != null)
            {
                return typeDocumentation.Description;
            }

            return String.Empty;
        }

        public string GetDocumentation(MemberInfo member)
        {
            var memberDocumentation = member.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            if (memberDocumentation != null)
            {
                return memberDocumentation.Description;
            }

            return String.Empty;            
        }
    }
}
