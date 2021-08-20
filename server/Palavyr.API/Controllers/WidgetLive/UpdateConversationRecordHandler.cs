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
            logger.LogDebug("Adding completed conversation to the database.");
            var area = await configurationRepository.GetAreaById(accountId, convo.AreaIdentifier);

            var emailTemplateUsed = area.EmailTemplate;
            var email = convo.Email;
            var name = convo.Name;
            var phone = convo.PhoneNumber;
            var fallback = convo.Fallback;
            var isComplete = convo.IsComplete;

            var record = await convoHistoryRepository.GetConversationRecordById(convo.ConversationId);

            if (emailTemplateUsed != null)
            {
                record.EmailTemplateUsed = emailTemplateUsed;
            }

            if (email != null)
            {
                record.Email = email;
            }

            if (name != null)
            {
                record.Name = name;
            }

            if (phone != null)
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
        }
    }
}