using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FCP.Web.Api
{
    public abstract class ApiActionDescriptorFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ActionDescriptor = GetCustomHttpActionDescriptor(actionContext.ControllerContext,
                actionContext.ActionDescriptor) ?? actionContext.ActionDescriptor;

            base.OnActionExecuting(actionContext);
        }

        protected abstract HttpActionDescriptor GetCustomHttpActionDescriptor(HttpControllerContext controllerContext, HttpActionDescriptor actionDescriptor);
    }
}
