using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    [Route("api")]
    [ApiController]
    public class CreateNewAccountGoogleController : ControllerBase
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