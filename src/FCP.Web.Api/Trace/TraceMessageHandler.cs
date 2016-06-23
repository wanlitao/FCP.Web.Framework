using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace FCP.Web.Api
{
    /// <summary>
    /// Trace Http消息
    /// </summary>
    public abstract class TraceMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestTime = DateTime.UtcNow;
            var requestContent = await request.Content.ReadAsStringAsync();
            await TraceHttpRequestAsync(request, requestTime, requestContent);

            var response = await base.SendAsync(request, cancellationToken);
            await TraceHttpRequestResponseAsync(request, requestTime, requestContent, response, DateTime.UtcNow);

            return response;
        }

        /// <summary>
        /// 记录 Http请求及响应
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="requestTime">请求时间</param>
        /// <param name="requestContent">请求内容</param>
        /// <returns></returns>
        protected abstract Task TraceHttpRequestAsync(HttpRequestMessage request, DateTime requestTime, string requestContent);

        /// <summary>
        /// 记录 Http请求及响应
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="requestTime">请求时间</param>
        /// <param name="requestContent">请求内容</param>
        /// <param name="response">响应消息</param>
        /// <param name="responseTime">响应时间</param>
        /// <returns></returns>
        protected abstract Task TraceHttpRequestResponseAsync(HttpRequestMessage request, DateTime requestTime, string requestContent,
            HttpResponseMessage response, DateTime responseTime);
    }
}
