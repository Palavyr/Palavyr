using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateNewConversationHistoryHandler : IRequestHandler<CreateNewConversationHistoryRequest, CreateNewConversationHistoryResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<ConversationHistoryMeta> convoRecordStore;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<ConversationNode, WidgetNodeResource> mapper;
        private readonly IMapToNew<ConversationHistoryMeta, ConversationRecordResource> recordResourceMapper;
        private readonly IEndingSequenceAttacher endingSequenceAttacher;
        private readonly ILogger<CreateNewConversationHistoryHandler> logger;
        private readonly IEntityStore<Intent> intentStore;

        public CreateNewConversationHistoryHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<ConversationHistoryMeta> convoRecordStore,
            IAccountIdTransport accountIdTransport,
            IMapToNew<ConversationNode, WidgetNodeResource> mapper,
            IMapToNew<ConversationHistoryMeta, ConversationRecordResource> recordResourceMapper,
            IEndingSequenceAttacher endingSequenceAttacher,
            ILogger<CreateNewConversationHistoryHandler> logger,
            IEntityStore<Intent> intentStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.convoRecordStore = convoRecordStore;
            this.accountIdTransport = accountIdTransport;
            this.mapper = mapper;
            this.recordResourceMapper = recordResourceMapper;
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
                .Where(x => x.IntentId == intentId)
                .ToListAsync(convoNodeStore.CancellationToken);
            var completeConversation = endingSequenceAttacher.AttachEndingSequenceToNodeList(standardNodesNoTracking, intentId, accountIdTransport.AccountId);
            var widgetNodes = await mapper.MapMany(completeConversation, cancellationToken);

            var newConvoResource = NewConversationResource.CreateNew(widgetNodes.ToList());

            var intent = await intentStore.Get(intentId, s => s.IntentId);
            var newConversationRecord = ConversationHistoryMeta.CreateDefault(newConvoResource.ConversationId, accountIdTransport.AccountId, intent.IntentName, intentId);

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

            // TODO: Map from convoRecord to ConvoRecordResource
            // var resource = await recordResourceMapper.Map(newConversationRecord, cancellationToken);
            return new CreateNewConversationHistoryResponse(newConvoResource);
        }
    }

    public class ConversationRecordResource
    {
        public string ConversationId { get; set; } // This will be used when collecting enquiries. Then used to get the 
        public string ResponsePdfId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string IntentName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IntentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFallback { get; set; }
        public string Locale { get; set; } // TODO: Correct This
        public bool IsComplete { get; set; }
    }

    public class ConversationRecordResourceMapper : IMapToNew<ConversationHistoryMeta, ConversationRecordResource>
    {
        public async Task<ConversationRecordResource> Map(ConversationHistoryMeta from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return new ConversationRecordResource
            {
                ConversationId = from.ConversationId, // This will be used when collecting enquiries. Then used to get the 
                ResponsePdfId = from.ResponsePdfId,
                TimeStamp = from.TimeStamp,
                IntentName = from.IntentName,
                EmailTemplateUsed = from.EmailTemplateUsed,
                Seen = from.Seen,
                Name = from.Name,
                Email = from.Email,
                PhoneNumber = from.PhoneNumber,
                IntentId = from.IntentId,
                IsDeleted = from.IsDeleted,
                IsFallback = from.IsFallback,
                Locale = from.Locale, // TODO: Correct This
                IsComplete = from.IsComplete
            };
        }
    }

    public class CreateNewConversationHistoryResponse
    {
        public NewConversationResource Response { get; set; }
        public CreateNewConversationHistoryResponse(NewConversationResource response) => Response = response;
    }

    public class CreateNewConversationHistoryRequest : IRequest<CreateNewConversationHistoryResponse>
    {
        public const string Route = "widget/create";

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