using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests.Registration;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class CreateNewAccountDefaultController : PalavyrBaseController
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
        public async Task<Credentials> Create(
            [FromBody] AccountDetails newAccountDetails,
            CancellationToken cancellationToken)
        {
            var credentials = await setupService.CreateNewAccountViaDefaultAsync(newAccountDetails, cancellationToken);
            return credentials;
        }
    }
}