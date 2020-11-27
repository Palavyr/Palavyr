using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes.Registration;
using Palavyr.API.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{ 
    [Route("api")]
    [ApiController]
    public class CreateNewAccountDefaultController : ControllerBase
    {
        private readonly IAccountSetupService setupService;

        public CreateNewAccountDefaultController(
            IAccountSetupService setupService
        )
        {
            this.setupService = setupService;
        }
        
        [AllowAnonymous]
        [HttpPost("account/create/default")]
        public async Task<IActionResult> Create([FromBody] AccountDetails newAccountDetails)
        {
            var credentials = await setupService.CreateNewAccountViaDefaultAsync(newAccountDetails);
            return Ok(credentials);
        }
    }
}