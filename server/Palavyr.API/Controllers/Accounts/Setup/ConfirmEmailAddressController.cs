using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{

    public class ConfirmEmailAddressController : PalavyrBaseController
    {
        private readonly IEmailVerificationService emailVerificationService;

        public ConfirmEmailAddressController(IEmailVerificationService emailVerificationService)
        {
            this.emailVerificationService = emailVerificationService;
        }

        [HttpPost("account/confirmation/{authToken}/action/setup")]
        public async Task<bool> Post([FromRoute] string authToken, CancellationToken cancellationToken)
        {
            var confirmed = await emailVerificationService.ConfirmEmailAddressAsync(authToken.Trim(), cancellationToken);
            return confirmed;
        }
    }
}