using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetMissingIntroductionSequenceTreeErrors : PalavyrBaseController
    {
        public const string Route = "configure-conversations/intro/tree-errors";
        private readonly IMediator mediator;

        public GetMissingIntroductionSequenceTreeErrors(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<TreeErrorsResource> GetIntro(
            [FromBody]
            GetMissingIntroductionSequenceTreeErrorsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Resource;
        }
    }
}