using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Web.Mvc;
using FCP.Util;
using System;
using System.Linq;

namespace FCP.Web
{
    /// <summary>
    /// Action过滤助手
    /// </summary>
    public class ActionFilterHelper
    {
        /// <summary>
        /// Action过滤缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<Filter>> _actionFilterCache = new ConcurrentDictionary<string, List<Filter>>();

        /// <summary>
        /// 从缓存中读取Action筛选集合
        /// </summary>
        /// <param name="controllerContext">控制器上下文</param>
        /// <param name="actionDescriptor">动作描述符</param>
        /// <returns></returns>
        public static IList<Filter> getActionFiltersFromCache(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            List<Filter> actionFilterList;
            if (!_actionFilterCache.TryGetValue(actionDescriptor.UniqueId, out actionFilterList))
            {
                actionFilterList = actionFilterList ?? new List<Filter>();
                var actionFilters = FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor);
                if (actionFilters.isNotEmpty())
                {
                    actionFilterList.AddRange(actionFilters);
                }
                _actionFilterCache[actionDescriptor.UniqueId] = actionFilterList;
            }
            return actionFilterList;
        }

        /// <summary>
        /// 判断Action是否包含指定筛选类型
        /// </summary>
        /// <param name="controllerContext">控制器上下文</param>
        /// <param name="actionDescriptor">动作描述符</param>
        /// <param name="filterType">筛选类型</param>
        /// <returns></returns>
        public static bool checkActionHasFilter(ControllerContext controllerContext, ActionDescriptor actionDescriptor, Type filterType)
        {
            var actionFilters = getActionFiltersFromCache(controllerContext, actionDescriptor);
            return actionFilters.Count(m => m.Instance.GetType() == filterType) > 0;
        }
    }
}