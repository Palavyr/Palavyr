using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateNewConversationHistoryHandler : IRequestHandler<CreateNewConversationHistoryRequest, CreateNewConversationHistoryResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<ConversationNode, WidgetNodeResource> mapper;
        private readonly IEndingSequenceAttacher endingSequenceAttacher;
        private readonly ILogger<CreateNewConversationHistoryHandler> logger;
        private readonly IEntityStore<Area> intentStore;

        public CreateNewConversationHistoryHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<ConversationRecord> convoRecordStore,
            IAccountIdTransport accountIdTransport,
            IMapToNew<ConversationNode, WidgetNodeResource> mapper,
            IEndingSequenceAttacher endingSequenceAttacher,
            ILogger<CreateNewConversationHistoryHandler> logger,
            IEntityStore<Area> intentStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.convoRecordStore = convoRecordStore;
            this.accountIdTransport = accountIdTransport;
            this.mapper = mapper;
            this.endingSequenceAttacher = endingSequenceAttacher;
            this.logger = logger;
            this.intentStore = intentStore;
        }

        public async Task<CreateNewConversationHistoryResponse> Handle(CreateNewConversationHistoryRequest recordUpdate, CancellationToken cancellationToken)
        {
            var intentId = recordUpdate.IntentId;

            // Need to use a no tracking query executor here so that we don't save changes to the nodeChildren string when we wire up the standard nodes to the ending sequence.
            var standardNodesNoTracking = await convoNodeStore
                .RawReadonlyQuery()
                .Where(x => x.AccountId == convoNodeStore.AccountId)
                .Where(x => x.AreaIdentifier == intentId)
                .ToListAsync(convoNodeStore.CancellationToken);
            var completeConversation = endingSequenceAttacher.AttachEndingSequenceToNodeList(standardNodesNoTracking, intentId, accountIdTransport.AccountId);
            var widgetNodes = await mapper.MapMany(completeConversation);

            var newConvo = NewConversation.CreateNew(widgetNodes.ToList());

            var intent = await intentStore.Get(intentId, s => s.AreaIdentifier);
            var newConversationRecord = ConversationRecord.CreateDefault(newConvo.ConversationId, accountIdTransport.AccountId, intent.AreaName, intentId);

            if (!string.IsNullOrEmpty(recordUpdate.Email))
            {
                newConversationRecord.Email = recordUpdate.Email;
            }

            if (!string.IsNullOrEmpty(recordUpdate.Name))
            {
                newConversationRecord.Name = recordUpdate.Name;
            }

            if (!recordUpdate.IsDemo)
            {
                await convoRecordStore.Create(newConversationRecord);
            }

            return new CreateNewConversationHistoryResponse(newConvo);
        }
    }

    public class CreateNewConversationHistoryResponse
    {
        public NewConversation Response { get; set; }
        public CreateNewConversationHistoryResponse(NewConversation response) => Response = response;
    }

    public class CreateNewConversationHistoryRequest : IRequest<CreateNewConversationHistoryResponse>
    {
        public string IntentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsDemo { get; set; }

        public void Deconstruct(out string intentId, out string name, out string email)
        {
            intentId = IntentId;
            name = Name;
            email = Email;
        }
    }
}