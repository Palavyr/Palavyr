// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Palavyr.Core.Data;
// using Palavyr.Core.Models.Resources.Requests;
// using Palavyr.Core.Services.AuthenticationServices;
//
// namespace Palavyr.API.Controllers.WidgetLive
// {
//     [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
//
//     public class SaveEmailWasSentController : PalavyrBaseController
//     {
//         private ILogger<SaveEmailWasSentController> logger;
//         private DashContext dashContext;
//         private ConvoContext convoContext;
//
//         public SaveEmailWasSentController(
//             ILogger<SaveEmailWasSentController> logger,
//             DashContext dashContext,
//             ConvoContext convoContext
//         )
//         {
//             this.logger = logger;
//             this.dashContext = dashContext;
//             this.convoContext = convoContext;
//         }
//
//         [HttpPost("widget/complete")]
//         public async Task<IActionResult> Post(
//             [FromHeader] string accountId,
//             CompleteConversation completeConvo)
//         {
//             logger.LogDebug("Adding completed conversation to the database.");
//             var area = await dashContext.Areas.SingleOrDefaultAsync(row =>
//                 row.AccountId == accountId && row.AreaIdentifier == completeConvo.AreaIdentifier);
//             var areaName = area.AreaName;
//             var emailTemplateUsed = area.EmailTemplate;
//
//             var conversationId = completeConvo.ConversationId;
//             var email = completeConvo.Email;
//             var name = completeConvo.Name;
//             var phone = completeConvo.PhoneNumber;
//             var hasResponse = completeConvo.HasResponse;
//             var fallback = completeConvo.Fallback;
//             var areaIdentifier = completeConvo.AreaIdentifier;
//             
//             var completedConversation = CompleteConversation.BindReceiverToSchemaType(
//                 conversationId, 
//                 accountId,
//                 areaName,
//                 emailTemplateUsed, 
//                 name, 
//                 email, 
//                 phone,
//                 hasResponse,
//                 fallback,
//                 areaIdentifier);
//
//             // do a quick check to see if the conversation ID already exists. If it does, delete it -- later  we should throw an exception
//             // TODO: Throw an exception
//             var existingConvo = convoContext.ConversationRecords.SingleOrDefault(row => row.ConversationId == conversationId);
//             if (existingConvo != null)
//             {
//                 convoContext.ConversationRecords.Remove(existingConvo);
//                 await convoContext.SaveChangesAsync();
//             }
//             
//             // TODO: Add validation on the phone number and the name perhaps
//             await convoContext.ConversationRecords.AddAsync(completedConversation);
//             await convoContext.SaveChangesAsync();
//             return NoContent();
//         }
//     }
// }