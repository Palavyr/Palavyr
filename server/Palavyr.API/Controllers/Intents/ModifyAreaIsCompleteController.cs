using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Areas
{
    public class ModifyAreaIsCompleteController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "areas/area-toggle";

        public ModifyAreaIsCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<bool> Put(ModifyIntentIsCompleteRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}