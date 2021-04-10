using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class CreateNewAccountGoogleController : PalavyrBaseController
    {
        private readonly IAccountSetupService setupService;

        public CreateNewAccountGoogleController(
            IAccountSetupService setupService
        )
        {
            this.setupService = setupService;
        }

        [AllowAnonymous]
        [HttpPost("account/create/google")]
        public async Task<IActionResult> Create([FromBody] GoogleRegistrationDetails registrationDetails)
        {
            var credentials = await setupService.CreateNewAccountViaGoogleAsync(registrationDetails);
            return Ok(credentials);
        }
    }
}