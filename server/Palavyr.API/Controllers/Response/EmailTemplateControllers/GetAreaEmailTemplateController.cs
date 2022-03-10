using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class GetAreaEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/{intentId}/email-template";

        public GetAreaEmailTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> GetEmailTemplate(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAreaEmailTemplateRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}