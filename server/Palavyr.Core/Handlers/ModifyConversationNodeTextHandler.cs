using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyConversationNodeTextHandler : IRequestHandler<ModifyConversationNodeTextRequest, ModifyConversationNodeTextResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyConversationNodeTextHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyConversationNodeTextResponse> Handle(ModifyConversationNodeTextRequest request, CancellationToken cancellationToken)
        {
            var updatedConversationNode = await configurationRepository.UpdateConversationNodeText(request.IntentId, request.NodeId, request.UpdatedNodeText);
            return new ModifyConversationNodeTextResponse(updatedConversationNode);
        }
    }

    public class ModifyConversationNodeTextResponse
    {
        public ModifyConversationNodeTextResponse(ConversationNode response) => Response = response;
        public ConversationNode Response { get; set; }
    }

    public class ModifyConversationNodeTextRequest : IRequest<ModifyConversationNodeTextResponse>
    {
        public string UpdatedNodeText { get; set; }
        public string NodeId { get; set; }
        public string IntentId { get; set; }
    }
}