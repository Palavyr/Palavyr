using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class GetDefaultFallbackEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/fallback/default-email-template";

        public GetDefaultFallbackEmailTemplateController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetDefaultFallbackEmailTemplateRequest(), cancellationToken);
            return response.Response;
        }
    }
}