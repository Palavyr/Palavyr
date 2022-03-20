using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class ResetMyPasswordController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "authentication/reset-my-password";


        public ResetMyPasswordController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost(Route)]
        public async Task<ResetPasswordResponse> Post([FromBody] ResetMyPasswordRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}