using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    public class PasswordResetRequestController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "authentication/password-reset-request";


        public PasswordResetRequestController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task<ResetResponse> Post([FromBody] PasswordRequestRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}