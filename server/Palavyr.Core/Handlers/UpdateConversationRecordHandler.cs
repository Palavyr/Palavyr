﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class UpdateConversationRecordHandler : INotificationHandler<UpdateConversationRecordRequest>
    {
        private readonly ILogger<UpdateConversationRecordHandler> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public UpdateConversationRecordHandler(
            ILogger<UpdateConversationRecordHandler> logger,
            IConfigurationRepository configurationRepository,
            IConvoHistoryRepository convoHistoryRepository)
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.convoHistoryRepository = convoHistoryRepository;
        }

        public async Task Handle(UpdateConversationRecordRequest request, CancellationToken cancellationToken)
        {
            var areaId = request.IntentId;
            var email = request.Email;
            var name = request.Name;
            var phone = request.PhoneNumber;
            var fallback = request.Fallback;
            var isComplete = request.IsComplete;

            var record = await convoHistoryRepository.GetConversationRecordById(request.ConversationId);

            if (!string.IsNullOrEmpty(areaId)) // we set this already when we create the convo, but here we use it to indicate if we've sent an email.
            {
                var area = await configurationRepository.GetAreaById(request.IntentId);
                record.EmailTemplateUsed = area.EmailTemplate;
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

            await configurationRepository.CommitChangesAsync();
            await convoHistoryRepository.CommitChangesAsync();
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