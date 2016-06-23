using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace FCP.Web
{
    /// <summary>
    /// Json.net实现 JsonResult
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// 默认Json序列化配置
        /// </summary>
        private JsonSerializerSettings defaultSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Error
        };


        private JsonSerializerSettings _serializerSettings;
        /// <summary>
        ///序列化配置
        /// </summary>
        public JsonSerializerSettings SerializerSettings
        {
            get
            {
                return _serializerSettings ?? defaultSerializerSettings;
            }
            set
            {
                _serializerSettings = value;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, SerializerSettings));
            }
        }
    }
}
