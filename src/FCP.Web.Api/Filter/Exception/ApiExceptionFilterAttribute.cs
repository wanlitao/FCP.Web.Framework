using System.Web.Http.Filters;

namespace FCP.Web.Api
{
    /// <summary>
    /// Api异常处理
    /// </summary>
    public abstract class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HandleApiException(actionExecutedContext);
            base.OnException(actionExecutedContext);
        }

        /// <summary>
        /// 处理Api异常
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        protected abstract void HandleApiException(HttpActionExecutedContext actionExecutedContext);
    }
}
