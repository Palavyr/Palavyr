using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Verification
{
    public class CheckEmailVerificationStatusController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "verification/email/status";


        public CheckEmailVerificationStatusController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<bool> RequestNewEmailVerification([FromBody] CheckEmailVerificationStatusRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}