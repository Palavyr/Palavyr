using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Enquiries
{
    public class ModifyCompletedConversationsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/update/{conversationId}";

        public ModifyCompletedConversationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<Enquiry[]> UpdateCompletedConversation(string conversationId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ModifyCompletedConversationsRequest(conversationId), cancellationToken);
            return response.Response;
        }
    }
}