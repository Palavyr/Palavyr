using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
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