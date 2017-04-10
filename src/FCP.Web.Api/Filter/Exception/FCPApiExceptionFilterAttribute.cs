using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace FCP.Web.Api
{
    public class FCPApiExceptionFilterAttribute : ApiExceptionFilterAttribute
    {
        protected override void HandleApiException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null && actionExecutedContext.Exception != null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, actionExecutedContext.Exception.Message, actionExecutedContext.Exception);
            }
        }
    }
}
