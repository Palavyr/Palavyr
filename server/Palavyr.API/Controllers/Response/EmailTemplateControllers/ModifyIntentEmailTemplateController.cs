using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class ModifyIntentEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;


        public ModifyIntentEmailTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(ModifyIntentEmailTemplateRequest.Route)]
        public async Task<string> Modify([FromBody] ModifyIntentEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}