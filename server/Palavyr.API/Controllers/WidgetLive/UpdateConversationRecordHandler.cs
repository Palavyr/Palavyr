using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.WidgetLive
{
    public interface IUpdateConversationRecordHandler
    {
        Task UpdateConversationRecord(string accountId, ConversationRecordUpdate convo);
    }


    public class UpdateConversationRecordHandler : IUpdateConversationRecordHandler
    {
        private readonly ILogger<UpdateConversationRecordHandler> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public UpdateConversationRecordHandler(
            ILogger<UpdateConversationRecordHandler> logger,
            IConfigurationRepository configurationRepository,
            IConvoHistoryRepository convoHistoryRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.convoHistoryRepository = convoHistoryRepository;
        }

        public async Task UpdateConversationRecord(string accountId, ConversationRecordUpdate convo)
        {
            var areaId = convo.AreaIdentifier;
            var email = convo.Email;
            var name = convo.Name;
            var phone = convo.PhoneNumber;
            var fallback = convo.Fallback;
            var isComplete = convo.IsComplete;

            var record = await convoHistoryRepository.GetConversationRecordById(convo.ConversationId);

            if (!string.IsNullOrEmpty(areaId)) // we set this already when we create the convo, but here we use it to indicate if we've sent an email.
            {
                var area = await configurationRepository.GetAreaById(accountId, convo.AreaIdentifier);
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
            
            ;
        }
    }
}