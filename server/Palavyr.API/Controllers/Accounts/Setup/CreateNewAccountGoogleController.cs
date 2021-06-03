using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
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
        public async Task<Credentials> Create([FromBody] GoogleRegistrationDetails registrationDetails, CancellationToken cancellationToken)
        {
            var credentials = await setupService.CreateNewAccountViaGoogleAsync(registrationDetails, cancellationToken);
            return credentials;
        }
    }
}