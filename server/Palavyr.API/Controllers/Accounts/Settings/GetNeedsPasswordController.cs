using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetNeedsPasswordController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/needs-password";


        public GetNeedsPasswordController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetNeedsPasswordRequest(), cancellationToken);
            return response.Response;
        }
    }
}