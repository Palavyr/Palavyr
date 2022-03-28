using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            
            // don't save changes when making modifications to the 
            var introConvo = await convoNodeStore.RawReadonlyQuery().Where(x => x.AreaIdentifier == account.IntroductionId).ToArrayAsync(cancellationToken);
            var intro = EndingSequenceAttacher.CleanTheIntroConvoEnding(introConvo);
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