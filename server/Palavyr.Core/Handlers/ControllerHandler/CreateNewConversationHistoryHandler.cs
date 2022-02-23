using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateNewConversationHistoryHandler : IRequestHandler<CreateNewConversationHistoryRequest, CreateNewConversationHistoryResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConvoHistoryRepository convoRepository;
        private readonly IHoldAnAccountId accountIdHolder;
        private readonly IMapToNew<ConversationNode, WidgetNodeResource> mapper;
        private readonly IEndingSequenceAttacher endingSequenceAttacher;
        private readonly ILogger<CreateNewConversationHistoryHandler> logger;

        public CreateNewConversationHistoryHandler(
            IConfigurationRepository configurationRepository,
            IConvoHistoryRepository convoRepository,
            IHoldAnAccountId accountIdHolder,
            IMapToNew<ConversationNode, WidgetNodeResource> mapper,
            IEndingSequenceAttacher endingSequenceAttacher,
            ILogger<CreateNewConversationHistoryHandler> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.convoRepository = convoRepository;
            this.accountIdHolder = accountIdHolder;
            this.mapper = mapper;
            this.endingSequenceAttacher = endingSequenceAttacher;
            this.logger = logger;
        }

        public async Task<CreateNewConversationHistoryResponse> Handle(CreateNewConversationHistoryRequest recordUpdate, CancellationToken cancellationToken)
        {
            var (intentId, name, email) = recordUpdate;

            logger.LogDebug("Fetching nodes...");
            var standardNodes = await configurationRepository.GetAreaConversationNodes(intentId);
            var completeConversation = endingSequenceAttacher.AttachEndingSequenceToNodeList(standardNodes, intentId, accountIdHolder.AccountId);

            logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");

            var widgetNodes = await mapper.MapMany(completeConversation, cancellationToken);
            
            var newConvo = NewConversation.CreateNew(widgetNodes.ToList());

            var area = await configurationRepository.GetAreaById(intentId);
            var newConversationRecord = ConversationRecord.CreateDefault(newConvo.ConversationId, accountIdHolder.AccountId, area.AreaName, intentId);

            if (!string.IsNullOrEmpty(recordUpdate.Email))
            {
                newConversationRecord.Email = recordUpdate.Email;
            }

            if (!string.IsNullOrEmpty(recordUpdate.Name))
            {
                newConversationRecord.Name = recordUpdate.Name;
            }

            await convoRepository.CreateNewConversationRecord(newConversationRecord);
            await convoRepository.CommitChangesAsync();

            return new CreateNewConversationHistoryResponse(newConvo);
        }
    }

    public class CreateNewConversationHistoryResponse
    {
        public NewConversation Response { get; set; }
        public CreateNewConversationHistoryResponse(NewConversation response) => Response = response;
    }

    public class CreateNewConversationHistoryRequest : IRequest<CreateNewConversationHistoryResponse>
    {
        public string IntentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public void Deconstruct(out string intentId, out string name, out string email)
        {
            intentId = IntentId;
            name = Name;
            email = Email;
        }
    }
}