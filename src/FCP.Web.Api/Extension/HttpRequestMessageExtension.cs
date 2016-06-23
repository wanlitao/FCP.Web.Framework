using System.Net.Http;

namespace FCP.Web.Api
{
    /// <summary>
    /// Http请求消息 扩展
    /// </summary>
    public static class HttpRequestMessageExtension
    {
        private const string httpContextKey = "MS_HttpContext";
        private const string remoteEndpointMessageKey = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string owinContextKey = "MS_OwinContext";

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string getClientIpAddress(this HttpRequestMessage request)
        {
            if (request == null)
                return string.Empty;

            // Web-hosting. Needs reference to System.Web.dll
            if (request.Properties.ContainsKey(httpContextKey))
            {
                dynamic ctx = request.Properties[httpContextKey];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            // Self-hosting. Needs reference to System.ServiceModel.dll. 
            if (request.Properties.ContainsKey(remoteEndpointMessageKey))
            {
                dynamic remoteEndpoint = request.Properties[remoteEndpointMessageKey];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
            if (request.Properties.ContainsKey(owinContextKey))
            {
                dynamic owinContext = request.Properties[owinContextKey];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }

            return string.Empty;
        }
    }
}
