using FCP.Core;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace FCP.Web.Api
{
    public class FCPDoResultConverter<T> : IActionResultConverter
    {
        public HttpResponseMessage Convert(HttpControllerContext controllerContext, object actionResult)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }

            HttpResponseMessage resultAsResponse = actionResult as HttpResponseMessage;
            if (resultAsResponse != null)
            {
                resultAsResponse.EnsureResponseHasRequest(controllerContext.Request);
                return resultAsResponse;
            }

            FCPDoResult<T> doResult = (FCPDoResult<T>)actionResult;
            if (doResult == null)
            {
                throw new InvalidOperationException("not return expected instance of generic FCPDoResult");
            }

            var statusCode = doResult.GetHttpStatusCode();
            if (doResult.isSuc)
            {
                return controllerContext.Request.CreateResponse(statusCode, doResult.data, controllerContext.Configuration);
            }
            else if (doResult.isValidFail)
            {
                return controllerContext.Request.CreateErrorResponse(statusCode, doResult.GetValidFailStateDictionary());
            }

            return controllerContext.Request.CreateErrorResponse(statusCode, doResult.msg ?? "request api error");
        }
    }
}
