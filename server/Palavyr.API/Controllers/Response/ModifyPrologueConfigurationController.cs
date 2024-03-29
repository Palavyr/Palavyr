using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response
{
    public class ModifyPrologueConfigurationController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/prologue";

        public ModifyPrologueConfigurationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<string> UpdatePrologue(
            [FromBody]
            ModifyPrologueConfigurationRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}