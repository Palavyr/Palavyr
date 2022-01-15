using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    public class GetAllAreasShallowController : PalavyrBaseController
    {
        public const string Route = "areas";
        public GetAllAreasShallowController()
        {
        }

        [HttpGet(Route)]
        public async Task<List<Area>> Get(CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new GetAllAreasRequest(), cancellationToken);
            return response.Response;
        }
    }
}