using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class UpdateConversationRecordController : PalavyrBaseController
    {
        private ILogger<UpdateConversationRecordController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public UpdateConversationRecordController(
            ILogger<UpdateConversationRecordController> logger,
            IConfigurationRepository configurationRepository,
            IConvoHistoryRepository convoHistoryRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.convoHistoryRepository = convoHistoryRepository;
        }

        [HttpPost("widget/record")]
        public async Task<IActionResult> Post(
            [FromHeader]
            string accountId,
            CompleteConversation completeConvo)
        {
            logger.LogDebug("Adding completed conversation to the database.");
            var area = await configurationRepository.GetAreaById(accountId, completeConvo.AreaIdentifier);
            var hasResponse = completeConvo.HasResponse;

            var emailTemplateUsed = area.EmailTemplate;
            var email = completeConvo.Email;
            var name = completeConvo.Name;
            var phone = completeConvo.PhoneNumber;
            var fallback = completeConvo.Fallback;


            var record = await convoHistoryRepository.GetConversationRecordById(completeConvo.ConversationId);

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


            await configurationRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}