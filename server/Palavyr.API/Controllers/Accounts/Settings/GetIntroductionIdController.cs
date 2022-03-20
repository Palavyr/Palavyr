using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetIntroductionIdController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "account/settings/intro-id";

        public GetIntroductionIdController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIntroductionIdRequest(), cancellationToken);
            return response.Response;
        }
    }
}