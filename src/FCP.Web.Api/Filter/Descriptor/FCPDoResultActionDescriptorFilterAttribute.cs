using System.Web.Http.Controllers;

namespace FCP.Web.Api
{
    public class FCPDoResultActionDescriptorFilterAttribute : ApiActionDescriptorFilterAttribute
    {
        protected override HttpActionDescriptor GetCustomHttpActionDescriptor(HttpControllerContext controllerContext, HttpActionDescriptor actionDescriptor)
        {
            return new FCPDoResultApiActionDescriptor(controllerContext, actionDescriptor);
        }
    }
}
