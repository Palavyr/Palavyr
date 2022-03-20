using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class ModifyAreaEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/email-template";


        public ModifyAreaEmailTemplateController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<string> Modify([FromBody] ModifyAreaEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}