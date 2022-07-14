using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers
{
    [Route(BaseRoute)]
    [ApiController]
    public abstract class PalavyrBaseController : ControllerBase
    {
        public const string BaseRoute = "api";

    }
}