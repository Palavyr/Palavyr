using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Palavyr.Core.Models;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetIntroductionSequenceController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;

        public GetIntroductionSequenceController(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
        }


        [HttpGet("account/settings/intro-sequence")]
        public async Task<ConversationNode[]> Get()
        {
            var account = await accountRepository.GetAccount();
            var introConvo = await configurationRepository.GetIntroductionSequence(account.IntroductionId);
            var intro = EndingSequence.CleanTheIntroConvoEnding(introConvo);
            return intro;
        }
    }


    public class GetIntroductionIdController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository repository;
        private readonly IHoldAnAccountId accountIdHolder;

        public GetIntroductionIdController(
            IConfigurationRepository configurationRepository,
            IAccountRepository repository,
            IHoldAnAccountId accountIdHolder
        )
        {
            this.configurationRepository = configurationRepository;
            this.repository = repository;
            this.accountIdHolder = accountIdHolder;
        }

        [HttpGet("account/settings/intro-id")]
        public async Task<string> Get()
        {
            var account = await repository.GetAccount();
            if (account.IntroductionId == null || string.IsNullOrEmpty(account.IntroductionId))
            {
                var newId = new GuidUtils().CreateNewId();
                account.IntroductionId = newId;
                await repository.CommitChangesAsync();

                var introSequence = ConversationNode.CreateDefaultRootNode(newId,  accountIdHolder.AccountId);
                await configurationRepository.CreateIntroductionSequence(introSequence);
                await configurationRepository.CommitChangesAsync();
                return newId;
            }

            return account.IntroductionId;
        }

        [HttpPost("account/settings/intro-id")]
        public async Task<ConversationNode[]> Post(
            [FromBody]
            ConversationNodeDto update)
        {
            var account = await repository.GetAccount();
            var accountId = accountIdHolder.AccountId;
            foreach (var node in update.Transactions)
            {
                node.AccountId = accountId;
            }
            var updatedConvo = await configurationRepository.UpdateIntroductionSequence(account.IntroductionId, update.Transactions);
            return updatedConvo;
        }
    }
}