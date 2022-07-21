using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class UpdateChatHistoryController : PalavyrBaseController
    {
        private readonly IMediator mediator;


        public UpdateChatHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(UpdateChatHistoryRequest.Route)]
        public async Task Modify(ConversationHistoryRowResource request, CancellationToken cancellationToken)
        {
            await mediator.Publish(new UpdateChatHistoryRequest(request), cancellationToken);
        }
    }
}