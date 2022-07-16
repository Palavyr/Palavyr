using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UpdateConversationRecordHandler : INotificationHandler<UpdateConversationRecordRequest>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<ConversationHistoryMeta> convoRecordStore;
        private readonly ILogger<UpdateConversationRecordHandler> logger;

        public UpdateConversationRecordHandler(
            IEntityStore<Intent> intentStore,
            IEntityStore<ConversationHistoryMeta> convoRecordStore,
            ILogger<UpdateConversationRecordHandler> logger)
        {
            this.intentStore = intentStore;
            this.convoRecordStore = convoRecordStore;
            this.logger = logger;
        }

        public async Task Handle(UpdateConversationRecordRequest request, CancellationToken cancellationToken)
        {
            var intentId = request.IntentId;
            var email = request.Email;
            var name = request.Name;
            var phone = request.PhoneNumber;
            var fallback = request.Fallback;
            var isComplete = request.IsComplete;

            var record = await convoRecordStore.Get(request.ConversationId, s => s.ConversationId);

            if (!string.IsNullOrEmpty(intentId)) // we set this already when we create the convo, but here we use it to indicate if we've sent an email.
            {
                var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
                record.EmailTemplateUsed = intent.EmailTemplate;
            }

            if (!string.IsNullOrEmpty(email))
            {
                record.Email = email;
            }

            if (!string.IsNullOrEmpty(name))
            {
                record.Name = name;
            }

            if (!string.IsNullOrEmpty(phone))
            {
                record.PhoneNumber = phone;
            }

            if (fallback != null)
            {
                record.IsFallback = fallback;
            }

            if (isComplete != null)
            {
                record.IsComplete = isComplete;
            }
        }
    }


    public class UpdateConversationRecordResponse
    {
        public UpdateConversationRecordResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class UpdateConversationRecordRequest : INotification
    {
        public string ConversationId { get; set; }
        public string IntentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Locale { get; set; }
        public bool Fallback { get; set; }
        public bool IsComplete { get; set; }
    }
}