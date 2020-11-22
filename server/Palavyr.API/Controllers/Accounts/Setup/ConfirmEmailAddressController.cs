using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class ConfirmEmailAddressController : ControllerBase
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