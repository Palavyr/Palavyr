using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Verification
{
    public class EmailVerificationController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "verification/email";

        public EmailVerificationController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<EmailVerificationResponse> RequestNewEmailVerification(
            [FromBody]
            EmailAddressVerificationRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}