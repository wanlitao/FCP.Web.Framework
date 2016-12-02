using FCP.Web.Api;
using System.Net.Http;
using System.Web.Http.Routing;
using WebApi.HelpPage.Controllers;
using FCP.Util;

namespace FCP.Web.Host
{
    /// <summary>
    /// FCP Help控制器
    /// </summary>
    [ApiLogActionIgnore]
    public abstract class ApiHelpController : HelpControllerBase
    {
        private readonly HttpRouteValueDictionary helpRouteValues;
        private string baseHelpRouteUrl = null;

        public ApiHelpController()
        {
            helpRouteValues = new HttpRouteValueDictionary(new { controller = helpControllerName });
            helpRouteValues.Add(HttpRoute.HttpRouteKey, true);
        }

        protected override string HelpRoute
        {
            get
            {
                baseHelpRouteUrl = baseHelpRouteUrl ?? getBaseHelpRouteUrl();

                if (helpRoutePrefix.isNullOrEmpty())
                    return baseHelpRouteUrl;

                return string.Format("/{0}{1}", helpRoutePrefix, baseHelpRouteUrl);
            }
        }

        /// <summary>
        /// 获取基础Help路由Url
        /// </summary>
        /// <returns></returns>
        private string getBaseHelpRouteUrl()
        {
            IHttpVirtualPathData virtualPathData = Configuration.Routes.GetVirtualPath(
                    new HttpRequestMessage(),
                    helpRouteName, helpRouteValues);

            if (virtualPathData == null)
                return string.Empty;

            return virtualPathData.VirtualPath;            
        }

        /// <summary>
        /// help控制器名称
        /// </summary>
        protected abstract string helpControllerName { get; }

        /// <summary>
        /// help路由名称
        /// </summary>
        protected abstract string helpRouteName { get; }

        /// <summary>
        /// help路由前缀（用于代理访问时拼装页面链接）
        /// </summary>
        protected abstract string helpRoutePrefix { get; }
    }
}
