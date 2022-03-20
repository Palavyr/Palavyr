using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntroductionSequenceHandler : IRequestHandler<GetIntroductionSequenceRequest, GetIntroductionSequenceResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public GetIntroductionSequenceHandler(IEntityStore<Account> accountStore, IEntityStore<ConversationNode> convoNodeStore)
        {
            this.accountStore = accountStore;
            this.convoNodeStore = convoNodeStore;
        }

        public async Task<GetIntroductionSequenceResponse> Handle(GetIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            var introConvo = await convoNodeStore.GetMany(account.IntroductionId, s => s.AreaIdentifier);
            var intro = EndingSequenceAttacher.CleanTheIntroConvoEnding(introConvo.ToArray());
            return new GetIntroductionSequenceResponse(intro);
        }
    }

    public class GetIntroductionSequenceResponse
    {
        public GetIntroductionSequenceResponse(ConversationNode[] response) => Response = response;
        public ConversationNode[] Response { get; set; }
    }

    public class GetIntroductionSequenceRequest : IRequest<GetIntroductionSequenceResponse>
    {
    }
}