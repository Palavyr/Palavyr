using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class UpdateConversationRecordController : PalavyrBaseController
    {
        private ILogger<UpdateConversationRecordController> logger;
        private readonly IUpdateConversationRecordHandler updateHandler;

        public UpdateConversationRecordController(
            ILogger<UpdateConversationRecordController> logger,
            IUpdateConversationRecordHandler updateHandler)
        {
            this.logger = logger;
            this.updateHandler = updateHandler;
        }

        [HttpPost("widget/record")]
        public async Task<IActionResult> Post(
            [FromHeader]
            string accountId,
            ConversationRecordUpdate convo)
        {
            updateHandler.UpdateConversationRecord(accountId, convo);
            return NoContent();
        }
    }
}