using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class CreateNewConversationHistoryController : PalavyrBaseController
    {
        public const string Route = "widget/create";

        public CreateNewConversationHistoryController()
        {
        }

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