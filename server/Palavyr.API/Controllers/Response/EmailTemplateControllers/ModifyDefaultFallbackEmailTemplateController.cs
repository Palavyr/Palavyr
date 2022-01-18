using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    public class ModifyDefaultFallbackEmailTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/fallback/default-email-template";

        public ModifyDefaultFallbackEmailTemplateController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<string> Modify([FromBody] ModifyDefaultFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}