using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntroductionSequenceHandler : IRequestHandler<ModifyIntroductionSequenceRequest, ModifyIntroductionSequenceResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<Account> accountStore;
        private readonly IMapToNew<ConversationDesignerNodeResource, ConversationNode> conversationNodeMapper;

        public ModifyIntroductionSequenceHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<Account> accountStore,
            IMapToNew<ConversationDesignerNodeResource, ConversationNode> conversationNodeMapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.accountStore = accountStore;
            this.conversationNodeMapper = conversationNodeMapper;
        }

        public async Task<ModifyIntroductionSequenceResponse> Handle(ModifyIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var conversationNodes = await conversationNodeMapper
                .MapMany(request.Transactions, cancellationToken);

            var account = await accountStore.GetAccount();
            await convoNodeStore.Delete(account.IntroIntentId, s => s.IntentId);

            await convoNodeStore.CreateMany(conversationNodes);
            return new ModifyIntroductionSequenceResponse(request.Transactions.ToArray());
        }
    }

    public class ModifyIntroductionSequenceResponse
    {
        public ModifyIntroductionSequenceResponse(ConversationDesignerNodeResource[] response) => Response = response;
        public ConversationDesignerNodeResource[] Response { get; set; }
    }

    public class ModifyIntroductionSequenceRequest : IRequest<ModifyIntroductionSequenceResponse>
    {
        public List<ConversationDesignerNodeResource> Transactions { get; set; }
    }
}