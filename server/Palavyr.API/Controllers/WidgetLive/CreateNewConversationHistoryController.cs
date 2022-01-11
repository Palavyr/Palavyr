using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class CreateNewConversationHistoryController : PalavyrBaseController
    {
        public CreateNewConversationHistoryController()
        {
        }

        public const string Route = "widget/create";

        [HttpPost(Route)]
        public async Task<NewConversation> Create(
            [FromBody]
            CreateNewConversationHistoryRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await Mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}