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
        public async Task<IActionResult> Post([FromRoute] string authToken)
        {
            var confirmed = await emailVerificationService.ConfirmEmailAddressAsync(authToken);
            return Ok(confirmed);
        }
    }
}