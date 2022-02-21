using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConversationHandler : IRequestHandler<GetConversationRequest, GetConversationResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetConversationHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetConversationResponse> Handle(GetConversationRequest request, CancellationToken cancellationToken)
        {
            var conversation = await configurationRepository.GetAreaConversationNodes(request.IntentId);
            return new GetConversationResponse(conversation);
        }
    }

    public class GetConversationResponse
    {
        public GetConversationResponse(List<ConversationNode> response) => Response = response;
        public List<ConversationNode> Response { get; set; }
    }

    public class GetConversationRequest : IRequest<GetConversationResponse>
    {
        public GetConversationRequest(string intendId)
        {
            IntentId = intendId;
        }
        public string IntentId { get; set; }
    }
}