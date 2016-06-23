using System.Web;
using System.Web.Mvc;

namespace FCP.Web
{
    /// <summary>
    /// Renders a view called "NotFound" and sets the response status code to 404.
    /// View data is assigned for "RequestedUrl" and "ReferrerUrl".
    /// </summary>
    public class NotFoundViewResult : HttpNotFoundResult
    {
        /// <summary>
        /// 默认404视图名称
        /// </summary>
        private const string defaultViewName = "NotFound";

        public NotFoundViewResult()
        {
            ViewName = defaultViewName;
            ViewData = new ViewDataDictionary();
        }

        /// <summary>
        /// The name of the view to render. Defaults to "NotFound".
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// The view data passed to the NotFound view.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            ViewData["RequestedUrl"] = GetRequestedUrl(request);
            ViewData["ReferrerUrl"] = GetReferrerUrl(request, request.Url.OriginalString);

            response.StatusCode = 404;
            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;

            var viewResult = new ViewResult
            {
                ViewName = ViewName,
                ViewData = ViewData
            };
            response.Clear();
            viewResult.ExecuteResult(context);
        }

        private string GetRequestedUrl(HttpRequestBase request)
        {
            return request.RawUrl;
        }

        private string GetReferrerUrl(HttpRequestBase request, string url)
        {
            return request.UrlReferrer != null && request.UrlReferrer.OriginalString != url
                       ? request.UrlReferrer.OriginalString
                       : null;
        }
    }
}
