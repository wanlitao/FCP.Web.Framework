using System.Net.Http.Formatting;
using Newtonsoft.Json;
using FCP.Util;
using Newtonsoft.Json.Converters;

namespace FCP.Web.Api
{
    /// <summary>
    /// Json MediaType序列化
    /// </summary>
    public class JsonNetMediaTypeFormatter : JsonMediaTypeFormatter
    {
        protected const string defaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTimeFormat">时间格式化字符串</param>
        public JsonNetMediaTypeFormatter(string dateTimeFormat = null)
        {
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;

            if (dateTimeFormat.isNullOrEmpty())
            {
                dateTimeFormat = defaultDateTimeFormat;
            }            
            SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat });
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatter"></param>
        public JsonNetMediaTypeFormatter(JsonNetMediaTypeFormatter formatter)
            : base(formatter)
        {
            SerializerSettings = formatter.SerializerSettings;
        }
        #endregion
    }
}
