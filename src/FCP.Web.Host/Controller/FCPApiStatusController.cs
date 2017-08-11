using FCP.Web.Api;
using System.Web.Http;
using System.Web.Http.Description;

namespace FCP.Web.Host
{
    [AllowAnonymous]    
    [ApiLogActionIgnore]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FCPApiStatusController : ApiController
    {
        [Route(FCPWebHostConstants.ApiStatusRouteUrl)]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
