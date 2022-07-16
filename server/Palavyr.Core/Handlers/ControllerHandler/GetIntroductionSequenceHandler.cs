using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntroductionSequenceHandler : IRequestHandler<GetIntroductionSequenceRequest, GetIntroductionSequenceResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper;

        public GetIntroductionSequenceHandler(IEntityStore<Account> accountStore, IEntityStore<ConversationNode> convoNodeStore, IMapToNew<ConversationNode, ConversationDesignerNodeResource> mapper)
        {
            this.accountStore = accountStore;
            this.convoNodeStore = convoNodeStore;
            this.mapper = mapper;
        }

        public async Task<GetIntroductionSequenceResponse> Handle(GetIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();

            // don't save changes when making modifications to the 
            var introConvo = await convoNodeStore.RawReadonlyQuery().Where(x => x.AreaIdentifier == account.IntroductionId).ToArrayAsync(cancellationToken);
            var intro = EndingSequenceAttacher.CleanTheIntroConvoEnding(introConvo);

            var resource = await mapper.MapMany(intro, cancellationToken);
            return new GetIntroductionSequenceResponse(resource);
        }
    }

    public class GetIntroductionSequenceResponse
    {
        public GetIntroductionSequenceResponse(IEnumerable<ConversationDesignerNodeResource> response) => Response = response;
        public IEnumerable<ConversationDesignerNodeResource> Response { get; set; }
    }

    public class GetIntroductionSequenceRequest : IRequest<GetIntroductionSequenceResponse>
    {
    }
}