using FCP.Util;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace FCP.Web.Api
{
    public abstract class FCPApiLogActionFilterAttribute : ApiLogActionFilterAttribute
    {
        protected override async Task logApiActionAsync(DateTime requestTime, HttpActionExecutedContext actionExecutedContext, DateTime responseTime)
        {
            var traceWriter = actionExecutedContext.ActionContext.ActionDescriptor.Configuration.Services.GetTraceWriter();

            var category = GetTraceCategory(actionExecutedContext);
            var responseJson = await GetHttpResponseContentAsync(actionExecutedContext.Response);

            traceWriter.Trace(actionExecutedContext.Request, category, TraceLevel.Info,
                BuildTraceRecordAction(actionExecutedContext, responseJson, requestTime, responseTime));
        }

        #region Helper Functions
        /// <summary>
        /// 获取缓存的请求内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected static string GetHttpRequestContentCache(HttpRequestMessage request)
        {
            if (request == null)
                return string.Empty;

            if (!request.Properties.ContainsKey(FCPApiConstants.requestContentPropertyKey))
                return string.Empty;

            return TypeHelper.parseString(request.Properties[FCPApiConstants.requestContentPropertyKey]);
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected static async Task<string> GetHttpResponseContentAsync(HttpResponseMessage response)
        {
            if (response == null)
                return string.Empty;

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        #endregion

        #region Category
        protected abstract string GetTraceCategory(HttpActionExecutedContext actionExecutedContext);
        #endregion

        #region TraceRecord Actions
        protected static Action<TraceRecord> BuildTraceRecordAction_AddRequestInfo(HttpActionExecutedContext actionExecutedContext)
        {
            return (traceRecord) =>
            {
                var apiDesc = actionExecutedContext.ActionContext.ActionDescriptor.description();
                traceRecord.Properties.Add("ActionDescription", apiDesc);

                var requestJson = GetHttpRequestContentCache(actionExecutedContext.Request);
                traceRecord.Properties.Add("RequestJson", requestJson);

                var requestParameterJson = SerializerFactory.JsonSerializer.SerializeString(actionExecutedContext.ActionContext.ActionArguments);
                traceRecord.Properties.Add("ActionArguments", requestParameterJson);

                var requestIP = actionExecutedContext.Request.getClientIpAddressWithCheckProxy();
                traceRecord.Properties.Add("ClientIP", requestIP);
            };
        }

        protected static Action<TraceRecord> BuildTraceRecordAction_AddResponseInfo(HttpActionExecutedContext actionExecutedContext, string responseJson)
        {
            return (traceRecord) =>
            {
                if (actionExecutedContext.Response != null)
                {
                    traceRecord.Status = actionExecutedContext.Response.StatusCode;
                    traceRecord.Properties.Add("ResponseStatusCode", traceRecord.Status);

                    traceRecord.Properties.Add("ResponseJson", responseJson ?? string.Empty);
                }
            };
        }

        protected static Action<TraceRecord> BuildTraceRecordAction_AddExceptionInfo(HttpActionExecutedContext actionExecutedContext)
        {
            return (traceRecord) =>
            {
                if (actionExecutedContext.Exception != null)
                {
                    traceRecord.Exception = actionExecutedContext.Exception;
                }
            };
        }

        protected static Action<TraceRecord> BuildTraceRecordAction_AddPerformanceInfo(DateTime requestTime, DateTime responseTime)
        {
            return (traceRecord) =>
            {
                traceRecord.Properties.Add("RequestTime", requestTime);
                traceRecord.Properties.Add("ResponseTime", responseTime);
                traceRecord.Properties.Add("Duration", responseTime - requestTime);
            };
        }

        protected abstract Action<TraceRecord> BuildTraceRecordAction_AddExtendInfo(HttpActionExecutedContext actionExecutedContext,
            string responseJson, DateTime requestTime, DateTime responseTime);

        private Action<TraceRecord> BuildTraceRecordAction(HttpActionExecutedContext actionExecutedContext,
            string responseJson, DateTime requestTime, DateTime responseTime)
        {
            return (traceRecord) =>
            {
                BuildTraceRecordAction_AddRequestInfo(actionExecutedContext)(traceRecord);

                BuildTraceRecordAction_AddResponseInfo(actionExecutedContext, responseJson)(traceRecord);

                BuildTraceRecordAction_AddExceptionInfo(actionExecutedContext)(traceRecord);

                BuildTraceRecordAction_AddPerformanceInfo(requestTime, responseTime)(traceRecord);
                
                BuildTraceRecordAction_AddExtendInfo(actionExecutedContext, responseJson, requestTime, responseTime)(traceRecord);
            };
        }
        #endregion
    }
}
