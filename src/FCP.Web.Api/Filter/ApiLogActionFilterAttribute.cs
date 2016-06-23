using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System;
using System.Linq;
using FCP.Util;
using System.Threading.Tasks;

namespace FCP.Web.Api
{
    /// <summary>
    /// 记录Api Action执行
    /// </summary>
    public abstract class ApiLogActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            
            actionContext.Request.Properties[FCPApiConstants.requestTimePropertyKey] = DateTime.UtcNow.Ticks;  //保存请求时间            
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);            

            if (!checkActionLogIgnore(actionExecutedContext.ActionContext.ActionDescriptor)
                && actionExecutedContext.Request.Properties.ContainsKey(FCPApiConstants.requestTimePropertyKey))
            {
                var cacheRequestTimeTicks = actionExecutedContext.Request.Properties[FCPApiConstants.requestTimePropertyKey];
                var requestTime = new DateTime(TypeHelper.parseLong(cacheRequestTimeTicks), DateTimeKind.Utc);
                var responseTime = DateTime.UtcNow;

                logApiActionAsync(requestTime, actionExecutedContext, responseTime);
            }
        }

        /// <summary>
        /// 判断是否忽略记录Action日志
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected static bool checkActionLogIgnore(HttpActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false;

            var actionFilters = actionDescriptor.GetFilterPipeline();
            if (actionFilters.isEmpty())
                return false;

            return actionFilters.Count(m => m.Instance.GetType() == typeof(ApiLogActionIgnoreAttribute)) > 0;
        }

        /// <summary>
        /// 记录Action请求和响应
        /// </summary>
        /// <param name="requestTime">请求时间</param>
        /// <param name="actionExecutedContext"></param>
        /// <param name="responseTime">响应时间</param>
        /// <returns></returns>
        protected abstract Task logApiActionAsync(DateTime requestTime, HttpActionExecutedContext actionExecutedContext, DateTime responseTime);
    }
}
