using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    public class VerifyPasswordResetRequestController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "authentication/verify-password-reset/{token}";

        public VerifyPasswordResetRequestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task<VerificationResponse> Post([FromRoute] string token, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new VerifyPasswordResetRequestRequest(token), cancellationToken);
            return response.Response;
        }
    }
}