using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class ResendConfirmationTokenController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/confirmation/token/resend";


        public ResendConfirmationTokenController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }


        [HttpPost(Route)]
        public async Task<bool> Post(
            [FromBody]
            ResendConfirmationTokenRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}