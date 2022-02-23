using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntroductionSequenceHandler : IRequestHandler<ModifyIntroductionSequenceRequest, ModifyIntroductionSequenceResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;

        public ModifyIntroductionSequenceHandler(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyIntroductionSequenceResponse> Handle(ModifyIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            foreach (var node in request.Transactions)
            {
                node.AccountId = account.AccountId;
            }

            var updatedConvo = await configurationRepository.UpdateIntroductionSequence(account.IntroductionId, request.Transactions);
            return new ModifyIntroductionSequenceResponse(updatedConvo);
        }
    }

    public class ModifyIntroductionSequenceResponse
    {
        public ModifyIntroductionSequenceResponse(ConversationNode[] response) => Response = response;
        public ConversationNode[] Response { get; set; }
    }

    public class ModifyIntroductionSequenceRequest : IRequest<ModifyIntroductionSequenceResponse>
    {
        public List<ConversationNode> Transactions { get; set; }
    }
}