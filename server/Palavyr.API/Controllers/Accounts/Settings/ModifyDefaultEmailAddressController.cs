using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyDefaultEmailAddressController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/email";

        public ModifyDefaultEmailAddressController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<EmailVerificationResponse> Modify([FromBody] ModifyDefaultEmailAddressRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}