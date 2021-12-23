using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{

    public class GetAllAreasShallowController : PalavyrBaseController
    {

        public GetAllAreasShallowController()
        {

        }
        
        [HttpGet("areas")]
        public async Task<List<Area>> Get(GetAllAreasRequest request, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(request, cancellationToken);
            return response.AllAreasShallow;
        }
    }
}