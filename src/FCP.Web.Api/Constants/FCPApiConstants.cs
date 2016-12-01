namespace FCP.Web.Api
{
    /// <summary>
    /// FCP Api 常量
    /// </summary>
    public static class FCPApiConstants
    {
        /// <summary>
        /// 代理历史客户端IP 头部名称
        /// </summary>
        public const string httpForwardClientIpHeaderName = "X-Fcp-Forwarded-For";
        /// <summary>
        /// 代理服务Id 头部名称
        /// </summary>
        public const string httpForwardServiceIdHeaderName = "X-Fcp-Forwarded-To";
        /// <summary>
        /// 代理服务Code 头部名称
        /// </summary>
        public const string httpForwardServiceCodeHeaderName = "X-Fcp-Forwarded-To-Name";


        /// <summary>
        /// 代理标识 头部名称
        /// </summary>
        public const string httpProxyFlagHeaderName = "X-Fcp-Proxy";
        /// <summary>
        /// 代理标识 true值
        /// </summary>
        public const string httpProxyFlagTrueValue = "Y";

        /// <summary>
        /// 请求时间 属性Key
        /// </summary>
        public const string requestTimePropertyKey = "__action_request_time__";

        /// <summary>
        /// 请求内容 属性Key
        /// </summary>
        public const string requestContentPropertyKey = "__action_request_content__";
    }
}
