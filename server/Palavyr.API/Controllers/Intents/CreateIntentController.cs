using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Intents
{
    public class CreateAreaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "intents/create";
        public CreateAreaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<Area> Create(
            [FromBody]
            CreateAreaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}