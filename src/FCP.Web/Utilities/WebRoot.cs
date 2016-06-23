using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// Web路径
    /// </summary>
    public static class WebRoot
    {
        private static string s_AppRoot;
        private static string s_ApplicationPath;

        static WebRoot()
        {
            try
            {
                s_ApplicationPath = HttpRuntime.AppDomainAppPath;

                string url = VirtualPathUtility.RemoveTrailingSlash(HttpRuntime.AppDomainAppVirtualPath);
                if (url == "/")
                    s_AppRoot = string.Empty;
                else
                    s_AppRoot = url;
            }
            catch
            {
                s_AppRoot = string.Empty;
            }
        }

        /// <summary>
        /// 程序虚拟路径
        /// 获取本程序安装的虚拟路径，始终不以/结尾。如果安装在根目录，将返回空字符串
        /// </summary>
        public static string appRoot
        {
            get { return s_AppRoot; }
        }

        /// <summary>
        /// 应用程序物理根路径
        /// </summary>
        public static string applicationPath
        {
            get { return s_ApplicationPath; }
            internal set
            {
                s_ApplicationPath = value;
            }
        }

        /// <summary>
        /// 获取当前请求的原始URL
        /// </summary>
        public static string rawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }
        /// <summary>
        /// 获取当前请求的绝对URL
        /// </summary>
        public static string absoluteUri
        {
            get { return HttpContext.Current.Request.Url.AbsoluteUri; }
        }
        /// <summary>
        /// 获取当前查询信息
        /// </summary>
        public static string query
        {
            get { return HttpContext.Current.Request.Url.Query; }
        }
        /// <summary>
        /// 客户端上次请求的URL
        /// </summary>
        public static string urlReferrer
        {
            get
            {
                return HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.PathAndQuery : string.Empty;
            }
        }

        /// <summary>
        /// 获取URL的绝对路径
        /// </summary>
        public static string absolutePath
        {
            get { return HttpContext.Current.Request.Url.AbsolutePath; }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        public static string clientIP
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                }
                else
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
            }
        }

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        /// <remarks>Add By 万里涛 at 2014.4.15</remarks>
        public static string getMapPath(string strPath)
        {
            if (HttpContext.Current != null && !string.IsNullOrEmpty(strPath))
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取服务器域名或IP地址和端口
        /// </summary>
        /// <remarks>Add By 万里涛 at 2014.4.16</remarks>
        public static string serverHost
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        public static string serverIP
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.ServerVariables["LOCAl_ADDR"];
                }
                return string.Empty;
            }
        }
    }
}
