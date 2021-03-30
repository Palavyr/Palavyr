using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class SaveCompletedConversationController : PalavyrBaseController
    {
        private ILogger<SaveCompletedConversationController> logger;
        private DashContext dashContext;
        private ConvoContext convoContext;

        public SaveCompletedConversationController(
            ILogger<SaveCompletedConversationController> logger,
            DashContext dashContext,
            ConvoContext convoContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.convoContext = convoContext;
        }

        [HttpPost("widget/complete")]
        public async Task<IActionResult> Post(
            [FromHeader] string accountId,
            CompleteConversation completeConvo)
        {
            logger.LogDebug("Adding completed conversation to the database.");
            var area = await dashContext.Areas.SingleOrDefaultAsync(row =>
                row.AccountId == accountId && row.AreaIdentifier == completeConvo.AreaIdentifier);
            var areaName = area.AreaName;
            var emailTemplateUsed = area.EmailTemplate;

            var conversationId = completeConvo.ConversationId;
            var email = completeConvo.Email;
            var name = completeConvo.Name;
            var phone = completeConvo.PhoneNumber;

            var completedConversation = CompleteConversation.BindReceiverToSchemaType(conversationId, accountId,
                areaName, emailTemplateUsed, name, email, phone);

            // do a quick check to see if the conversation ID already exists. If it does, delete it -- later  we should throw an exception
            // TODO: Throw an exception
            var existingConvo = convoContext.CompletedConversations.SingleOrDefault(row => row.ConversationId == conversationId);
            if (existingConvo != null)
            {
                convoContext.CompletedConversations.Remove(existingConvo);
                await convoContext.SaveChangesAsync();

            }
            
            // TODO: Add validation on the phone number and the name perhaps
            await convoContext.CompletedConversations.AddAsync(completedConversation);
            await convoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}