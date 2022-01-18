using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public abstract class PalavyrBaseController : ControllerBase
    {
    }
}