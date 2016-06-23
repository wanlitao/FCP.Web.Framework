using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FCP.Util;

namespace FCP.Web
{
    /// <summary>
    /// 404错误处理程序
    /// </summary>
    public class NotFoundHttpHandler : IHttpHandler
    {
        private static Func<IController> _createNotFoundControllerFunc = () => new NotFoundController();

        public static Func<IController> CreateNotFoundControllerFunc
        {
            get
            {
                return _createNotFoundControllerFunc;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _createNotFoundControllerFunc = value;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            try
            {
                var requestContext = CreateRequestContext(context);
                var controller = _createNotFoundControllerFunc();
                controller.Execute(requestContext);
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.FormatLogMessage());
            }            
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");
            var requestContext = new RequestContext(context, routeData);
            return requestContext;
        }

        public bool IsReusable
        {
            get { return false; }
        }        
    }
}
