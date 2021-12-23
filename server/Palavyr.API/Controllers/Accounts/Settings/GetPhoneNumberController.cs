using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetPhoneNumberController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private readonly IAccountRepository repository;
        private ILogger<GetPhoneNumberController> logger;
        public GetPhoneNumberController(IAccountRepository repository, ILogger<GetPhoneNumberController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        
        [HttpGet("account/settings/phone-number")]
        public async Task<IActionResult> Get()
        {
            var account = await repository.GetAccount();
            var phoneDetails = PhoneDetails.Create(account.PhoneNumber, account.Locale);
            return Ok(phoneDetails);
        }
    }
}