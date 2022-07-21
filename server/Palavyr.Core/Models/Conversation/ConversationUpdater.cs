using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Models.Conversation
{
    public class ConversationUpdater : IConversationNodeUpdater
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IOrphanRemover orphanRemover;
        private readonly IMapToNew<ConversationDesignerNodeResource, ConversationNode> mapper;

        public ConversationUpdater(
            IEntityStore<Intent> intentStore,
            IEntityStore<ConversationNode> convoNodeStore,
            IOrphanRemover orphanRemover,
            IMapToNew<ConversationDesignerNodeResource, ConversationNode> mapper) // TODO: This is not idea. We shouldn't be deleting the entire conversation - instead refactor to identify the same nodes and just update instead of delete and add.
        {
            this.intentStore = intentStore;
            this.convoNodeStore = convoNodeStore;
            this.orphanRemover = orphanRemover;
            this.mapper = mapper;
        }

        public async Task<List<ConversationNode>> UpdateDesignerConversationForIntent(string intentId, IEnumerable<ConversationNode> mappedUpdates, CancellationToken cancellationToken)
        {
            var deOrphanedIntentConvo = orphanRemover.RemoveOrphanedNodes(mappedUpdates);

            var intent = await intentStore.GetIntentComplete(intentId);
            var currentNodes = intent.ConversationNodes;
            await convoNodeStore.Delete(currentNodes);

            intent.ConversationNodes.AddRange(deOrphanedIntentConvo);

            return deOrphanedIntentConvo;
        }
    }
}