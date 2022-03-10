using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class GetAreaFallbackEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "email/fallback/{intentId}/email-template";


        public GetAreaFallbackEmailTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAreaFallbackEmailTemplateRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}