using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntroductionIdHandler : IRequestHandler<GetIntroductionIdRequest, GetIntroductionIdResponse>
    {
        private readonly IConfigurationEntityStore<ConversationNode> convoNodeStore;
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly IGuidUtils guidUtils;

        public GetIntroductionIdHandler(
            IConfigurationEntityStore<ConversationNode> convoNodeStore,
            IConfigurationEntityStore<Account> accountStore,
            IGuidUtils guidUtils
        )
        {
            this.convoNodeStore = convoNodeStore;
            this.accountStore = accountStore;
            this.guidUtils = guidUtils;
        }

        public async Task<GetIntroductionIdResponse> Handle(GetIntroductionIdRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            if (account.IntroductionId == null || string.IsNullOrEmpty(account.IntroductionId))
            {
                var newId = guidUtils.CreateNewId();
                account.IntroductionId = newId;

                var introSequence = ConversationNode.CreateDefaultRootNode(newId, account.AccountId);

                await convoNodeStore.CreateMany(introSequence.ToArray());
                return new GetIntroductionIdResponse(newId);
            }

            return new GetIntroductionIdResponse(account.IntroductionId);
        }
    }

    public class GetIntroductionIdResponse
    {
        public GetIntroductionIdResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetIntroductionIdRequest : IRequest<GetIntroductionIdResponse>
    {
    }
}