using System;

namespace FCP.Web.Host
{
    /// <summary>
    /// 自寄宿助手
    /// </summary>
    public static class SelfHostHelper
    {
        /// <summary>
        /// 获取可用寄宿Uri
        /// </summary>
        /// <returns></returns>
        public static Uri getFreeSelfHostUri(string ipAddressPrefix = null)
        {
            var localIpv4address = LocalHostHelper.getLocalIpv4Address(ipAddressPrefix);
            var freeTcpPort = LocalHostHelper.getFreeTcpPort();

            if (string.IsNullOrEmpty(localIpv4address))
                return null;

            var uriBuilder = new UriBuilder("http", localIpv4address, freeTcpPort);

            return uriBuilder.Uri;
        }
        
        /// <summary>
        /// 获取寄宿监听Url
        /// </summary>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static string getSelfHostListenUrl(int port)
        {
            if (port < 1)
                return string.Empty;

            return new UriBuilder("http", "+", port).ToString();
        } 
    }
}