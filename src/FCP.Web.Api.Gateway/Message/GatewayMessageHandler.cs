using System.Net.Http;
using System.Threading.Tasks;
using FCP.Util;
using System.Net;
using System.Web.Http;
using System.Threading;

namespace FCP.Web.Api.Gateway
{
    /// <summary>
    /// Http消息 网关
    /// </summary>
    public abstract class GatewayMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestSubSystemCode = await getRequestSubSystemCodeAsync(request);
            if (!requestSubSystemCode.isNullOrEmpty())            
            {
                request = await getSubSystemRequestAsync(request, requestSubSystemCode);
                if (request == null)  //未找到健康状态的 服务实例, 无法进行代理转发
                    throw new HttpResponseException(HttpStatusCode.NotFound);                

                addHttpRequestProxyFlagHeader(request);  //添加需要代理标识                              
            }
             
            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 添加Http请求 需要代理标识
        /// </summary>
        /// <param name="request"></param>
        protected void addHttpRequestProxyFlagHeader(HttpRequestMessage request)
        {
            if (request == null || request.Headers == null)
                return;

            var requestHeaders = request.Headers;
            var proxyFlagHeaderName = FCPApiConstants.httpProxyFlagHeaderName;

            if (requestHeaders.Contains(proxyFlagHeaderName))
            {
                requestHeaders.Remove(proxyFlagHeaderName);
            }
            requestHeaders.Add(proxyFlagHeaderName, FCPApiConstants.httpProxyFlagTrueValue);
        }

        /// <summary>
        /// 获取请求子系统编码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Task<string> getRequestSubSystemCodeAsync(HttpRequestMessage request);

        /// <summary>
        /// 生成子系统请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="subSystemCode">子系统编码</param>
        /// <returns></returns>
        protected abstract Task<HttpRequestMessage> getSubSystemRequestAsync(HttpRequestMessage request, string subSystemCode);
    }
}
