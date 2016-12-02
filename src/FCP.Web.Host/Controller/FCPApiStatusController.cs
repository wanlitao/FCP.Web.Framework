using FCP.Web.Api;
using System.Web.Http;
using System.Web.Http.Description;

namespace FCP.Web.Host
{
    [ApiLogActionIgnore]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FCPApiStatusController : ApiController
    {
        [Route(FCPWebHostConstants.apiStatusRouteUrl)]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
