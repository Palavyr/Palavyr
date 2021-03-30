using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class CreateAreaController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private ILogger<CreateAreaController> logger;

        public CreateAreaController(

            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            ILogger<CreateAreaController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        [HttpPost("areas/create")]
        public async Task<Area> Create([FromHeader] string accountId, [FromBody] AreaNameText areaNameText)
        {
            var account = await accountRepository.GetAccount(accountId);

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            var newArea = await configurationRepository.CreateAndAddNewArea(areaNameText.AreaName, accountId, defaultEmail, isVerified);
            await configurationRepository.CommitChangesAsync();
            return newArea;
        }
    }
}