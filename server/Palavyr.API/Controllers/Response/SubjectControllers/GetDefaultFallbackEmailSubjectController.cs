using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class GetDefaultFallbackEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/default-fallback-subject";


        public GetDefaultFallbackEmailSubjectController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Modify(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetDefaultFallbackEmailSubjectRequest(), cancellationToken);
            return response.Response;
        }
    }
}