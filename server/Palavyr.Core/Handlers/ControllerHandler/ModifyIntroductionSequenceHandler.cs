using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntroductionSequenceHandler : IRequestHandler<ModifyIntroductionSequenceRequest, ModifyIntroductionSequenceResponse>
    {
        private readonly IConfigurationEntityStore<ConversationNode> convoNodeStore;
        private readonly IConfigurationEntityStore<Account> accountStore;

        public ModifyIntroductionSequenceHandler(
            IConfigurationEntityStore<ConversationNode> convoNodeStore,
            IConfigurationEntityStore<Account> accountStore
        )
        {
            this.convoNodeStore = convoNodeStore;
            this.accountStore = accountStore;
        }

        public async Task<ModifyIntroductionSequenceResponse> Handle(ModifyIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            foreach (var node in request.Transactions)
            {
                node.AccountId = account.AccountId;
            }
            
            await convoNodeStore.Delete(account.IntroductionId, s => s.AreaIdentifier);
            await convoNodeStore.CreateMany(request.Transactions.ToArray());

            return new ModifyIntroductionSequenceResponse(request.Transactions.ToArray());
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