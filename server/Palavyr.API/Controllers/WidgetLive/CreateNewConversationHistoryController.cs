using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class CreateNewConversationHistoryController : PalavyrBaseController
    {
        private readonly IMediator mediator;


        public CreateNewConversationHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(CreateNewConversationHistoryRequest.Route)]
        public async Task<NewConversationResource> Create(
            [FromQuery]
            bool demo,
            [FromBody]
            CreateNewConversationHistoryRequest request,
            CancellationToken cancellationToken
        )
        {
            request.IsDemo = demo;
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}