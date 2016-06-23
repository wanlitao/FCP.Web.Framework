using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace FCP.Web.Host
{
    public static class LocalHostHelper
    {
        #region 获取本机IP地址
        /// <summary>
        /// 无效ip地址前缀
        /// </summary>
        public const string inValidIpAddressPrefix = "169.254";

        /// <summary>
        /// 获取本机IPv4地址
        /// </summary>
        /// <returns></returns>
        public static string getLocalIpv4Address(string ipAddressPrefix = null)
        {
            //从IP地址列表中筛选出IPv4类型的IP地址
            //AddressFamily.InterNetwork表示此IP为IPv4
            Predicate<IPAddress> ipv4AddressPredicate = ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork
               && ipAddress.ToString().StartsWith(ipAddressPrefix ?? string.Empty);                       

            return getLocalIpAddressByPredicate(ipv4AddressPredicate);
        }

        /// <summary>
        /// 获取本机IPv6地址
        /// </summary>
        /// <returns></returns>
        public static string getLocalIpv6Address()
        {
            //从IP地址列表中筛选出IPv6类型的IP地址            
            //AddressFamily.InterNetworkV6表示此地址为IPv6类型
            Predicate<IPAddress> ipv6AddressPredicate = ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetworkV6;

            return getLocalIpAddressByPredicate(ipv6AddressPredicate);
        }

        /// <summary>
        /// 根据Predicate获取本机IP地址
        /// </summary>
        /// <param name="ipAddressPredicate"></param>
        /// <returns></returns>
        private static string getLocalIpAddressByPredicate(Predicate<IPAddress> ipAddressPredicate)
        {
            var localIpAddressList = getLocalIpAddressList().Where(m => !m.ToString().StartsWith(inValidIpAddressPrefix));
            if (ipAddressPredicate != null)
            {
                localIpAddressList = localIpAddressList.Where(m => ipAddressPredicate(m));
            }
            var firstLocalIpAddress = localIpAddressList.FirstOrDefault();
            if (firstLocalIpAddress == null)
                return string.Empty;

            return firstLocalIpAddress.ToString();
        }

        /// <summary>
        /// 获取本机IP地址列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IPAddress> getLocalIpAddressList()
        {
            var hostName = Dns.GetHostName(); //得到主机名
            var hostEntry = Dns.GetHostEntry(hostName);

            return hostEntry.AddressList;
        }
        #endregion

        #region 获取TCP端口
        /// <summary>
        /// 获取可用TCP端口
        /// </summary>
        /// <returns></returns>
        public static int getFreeTcpPort()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();
            return port;
        }
        #endregion
    }
}