using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class UpdateConversationRecordController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "widget/record";

        public UpdateConversationRecordController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Post(
            UpdateConversationRecordRequest request,
            CancellationToken cancellationToken)
        {
            await mediator.Publish(request, cancellationToken);
        }
    }
}