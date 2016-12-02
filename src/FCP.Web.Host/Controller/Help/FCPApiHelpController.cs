using FCP.Util;
using FCP.Web.Api;

namespace FCP.Web.Host
{
    public abstract class FCPApiHelpController : ApiHelpController
    {
        public const string controllerRoute = "Help";

        public static string routeName
        {
            get
            {
                return string.Format("Default{0}", controllerRoute);
            }
        }

        /// <summary>
        /// help路由名称
        /// </summary>
        protected override string helpRouteName { get { return routeName; } }

        /// <summary>
        /// help路由前缀（用于代理访问时拼装页面链接）
        /// </summary>
        protected override string helpRoutePrefix
        {
            get
            {
                return getRequestForwardServiceCode();
            }
        }

        /// <summary>
        /// 获取请求代理的 服务Code
        /// </summary>
        /// <returns></returns>
        private string getRequestForwardServiceCode()
        {
            if (Request == null || Request.Headers.isEmpty())
                return string.Empty;

            return Request.Headers.GetLastValue(FCPApiConstants.httpForwardServiceCodeHeaderName, true);
        }
    }
}
