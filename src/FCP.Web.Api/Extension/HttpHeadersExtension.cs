using System.Collections.Generic;
using System.Net.Http.Headers;
using FCP.Util;
using System.Linq;

namespace FCP.Web.Api
{
    /// <summary>
    /// Http头部信息 扩展
    /// </summary>
    public static class HttpHeadersExtension
    {
        /// <summary>
        /// 添加或追加 头部信息值
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <param name="headerName">头部名称</param>
        /// <param name="value"></param>
        public static void AddOrAppend(this HttpHeaders httpHeaders, string headerName, string value)
        {
            if (headerName.isNullOrEmpty())
                return;

            var newHeaderValues = new List<string>();
            if (httpHeaders.Contains(headerName))
            {
                newHeaderValues.AddRange(httpHeaders.GetValues(headerName));
                httpHeaders.Remove(headerName);  //移除原来的 头部信息
            }

            newHeaderValues.Add(value); //添加值到最后            
            httpHeaders.Add(headerName, newHeaderValues);
        }

        /// <summary>
        /// 获取头部信息 首值
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <param name="headerName">头部名称</param>
        /// <param name="ignoreEmpty">是否忽略空值</param>
        /// <returns></returns>
        public static string GetFirstValue(this HttpHeaders httpHeaders, string headerName, bool ignoreEmpty)
        {
            var headerValues = httpHeaders.GetValues(headerName, ignoreEmpty);
            if (headerValues.isEmpty())
                return string.Empty;

            return headerValues.First();
        }

        /// <summary>
        /// 获取头部信息 末值
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <param name="headerName">头部名称</param>
        /// <param name="ignoreEmpty">是否忽略空值</param>
        /// <returns></returns>
        public static string GetLastValue(this HttpHeaders httpHeaders, string headerName, bool ignoreEmpty)
        {
            var headerValues = httpHeaders.GetValues(headerName, ignoreEmpty);
            if (headerValues.isEmpty())
                return string.Empty;

            return headerValues.Last();
        }

        /// <summary>
        /// 获取头部信息 值的集合
        /// </summary>
        /// <param name="httpHeaders"></param>
        /// <param name="headerName">头部名称</param>
        /// <param name="ignoreEmpty">是否忽略空值</param>
        /// <returns></returns>
        public static IEnumerable<string> GetValues(this HttpHeaders httpHeaders, string headerName, bool ignoreEmpty)
        {
            if (headerName.isNullOrEmpty())
                return null;

            if (!httpHeaders.Contains(headerName))
                return null;

            var headerValues = httpHeaders.GetValues(headerName);
            if (headerValues.isNotEmpty() && ignoreEmpty)
            {
                headerValues = headerValues.Where(m => !m.isNullOrEmpty());
            }
            return headerValues;
        }
    }
}
