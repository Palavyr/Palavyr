using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    public class CreateAreaController : PalavyrBaseController
    {
        public const string Route = "areas/create";
        public CreateAreaController()
        {
        }

        [HttpPost(Route)]
        public async Task<Area> Create(
            [FromBody]
            CreateAreaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}