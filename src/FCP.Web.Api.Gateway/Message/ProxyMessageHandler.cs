using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FCP.Util;

namespace FCP.Web.Api.Gateway
{
    /// <summary>
    /// Http消息 代理
    /// </summary>
    public class ProxyMessageHandler : DelegatingHandler
    {
        private const string connectionHeaderName = "Connection";        
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!isRequestShouldProxy(request))
                return await base.SendAsync(request, cancellationToken);

            editHttpRequestHeaders(request);

            //have to explicitly null it to avoid protocol violation
            if (request.Method == HttpMethod.Get)
                request.Content = null;

            var response = await new HttpClient().SendAsync(request, cancellationToken);
            editHttpResponseHeaders(response);

            return response;
        }

        /// <summary>
        /// 是否请求需要代理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected bool isRequestShouldProxy(HttpRequestMessage request)
        {
            if (request == null || request.Headers.isEmpty())
                return false;

            var requestHeaders = request.Headers;
            if (!requestHeaders.Contains(FCPApiConstants.httpProxyFlagHeaderName))
                return false;
            
            var proxyFlagValues = requestHeaders.GetValues(FCPApiConstants.httpProxyFlagHeaderName);
            if (proxyFlagValues.isEmpty())
                return false;

            return StringUtil.compareIgnoreCase(string.Join(",", proxyFlagValues), FCPApiConstants.httpProxyFlagTrueValue);
        }

        #region 修改头部信息

        #region 修改请求头部
        /// <summary>
        /// 编辑请求头部信息
        /// </summary>
        /// <param name="requestHeaders"></param>
        protected void editHttpRequestHeaders(HttpRequestMessage request)
        {
            if (request == null || request.Headers == null)
                return;

            var requestHeaders = request.Headers;

            requestHeaders.TransferEncoding.Clear();
            removeConnectionHttpHeaders(requestHeaders, requestHeaders.Connection.ToArray());
            requestHeaders.Remove(FCPApiConstants.httpProxyFlagHeaderName);  //移除 代理标识 头部信息
            
            requestHeaders.Host = request.RequestUri.Authority;  //更新请求Host            
            addHttpRequestHeaderClientIp(request);  //添加请求 客户端IP 

            customEditHttpRequestHeaders(request);
        }

        /// <summary>
        /// 自定义 编辑请求头部信息
        /// </summary>
        /// <param name="request"></param>
        protected virtual void customEditHttpRequestHeaders(HttpRequestMessage request)
        {

        }
        #endregion

        #region 修改响应头部
        /// <summary>
        /// 编辑响应头部信息
        /// </summary>
        /// <param name="requestHeaders"></param>
        protected void editHttpResponseHeaders(HttpResponseMessage response)
        {
            if (response == null || response.Headers == null)
                return;

            var responseHeaders = response.Headers;

            responseHeaders.TransferEncoding.Clear();
            removeConnectionHttpHeaders(responseHeaders, responseHeaders.Connection.ToArray());

            customEditHttpResponseHeaders(response);
        }

        /// <summary>
        /// 自定义 编辑响应头部信息
        /// </summary>
        /// <param name="response"></param>
        protected virtual void customEditHttpResponseHeaders(HttpResponseMessage response)
        {

        }        
        #endregion

        /// <summary>
        /// 移除connection头部信息
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="headerNames"></param>
        private static void removeConnectionHttpHeaders(HttpHeaders headers, params string[] headerNames)
        {
            if (headers.isEmpty())
                return;

            if (headerNames.isNotEmpty())
            {
                foreach (var headerName in headerNames)
                {
                    headers.Remove(headerName);
                }
            }            
            headers.Remove(connectionHeaderName);
        }

        /// <summary>
        /// 添加请求 客户端IP
        /// </summary>
        private static void addHttpRequestHeaderClientIp(HttpRequestMessage request)
        {
            if (request == null || request.Headers == null)
                return;

            var currentForwardClientIp = request.getClientIpAddress();
            if (currentForwardClientIp.isNullOrEmpty())
                return;

            request.Headers.AddOrAppend(FCPApiConstants.httpForwardClientIpHeaderName, currentForwardClientIp);
        }
        #endregion
    }
}
