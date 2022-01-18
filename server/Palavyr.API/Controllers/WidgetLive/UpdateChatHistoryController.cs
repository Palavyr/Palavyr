using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class UpdateChatHistoryController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/conversation";

        public UpdateChatHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Modify(UpdateChatHistoryRequest request, CancellationToken cancellationToken)
        {
            await mediator.Publish(request, cancellationToken);
        }
    }
}