using FCP.Util;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace FCP.Web
{
    /// <summary>
    /// 日志记录异常
    /// </summary>
    public class LogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null)
            {
                logMvcException(filterContext);
            }
            base.OnException(filterContext);
        }

        /// <summary>
        /// 记录MVC异常
        /// </summary>
        /// <param name="filterContext"></param>
        private static void logMvcException(ExceptionContext filterContext)
        {
            string areaName = filterContext.RouteData.DataTokens["area"] as string;
            string controllerName = filterContext.RouteData.Values["controller"] as string;
            string actionName = filterContext.RouteData.Values["action"] as string;

            string mvcErrorLogMessage = string.Format("发生异常 Area: {0} Controller：{1} Action：{2}",
                areaName, controllerName, actionName) + Environment.NewLine;
            mvcErrorLogMessage += filterContext.Exception.FormatLogMessage();  //添加异常信息

            Trace.TraceError(mvcErrorLogMessage);
        }
    }
}
