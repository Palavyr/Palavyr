using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class GetIntentEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/{intentId}/email-template";

        public GetIntentEmailTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> GetEmailTemplate(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIntentEmailTemplateRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}