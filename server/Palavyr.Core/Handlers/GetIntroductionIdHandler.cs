using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetIntroductionIdHandler : IRequestHandler<GetIntroductionIdRequest, GetIntroductionIdResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IGuidUtils guidUtils;

        public GetIntroductionIdHandler(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            IGuidUtils guidUtils
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.guidUtils = guidUtils;
        }

        public async Task<GetIntroductionIdResponse> Handle(GetIntroductionIdRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            if (account.IntroductionId == null || string.IsNullOrEmpty(account.IntroductionId))
            {
                var newId = guidUtils.CreateNewId();
                account.IntroductionId = newId;

                var introSequence = ConversationNode.CreateDefaultRootNode(newId, account.AccountId);
                await configurationRepository.CreateIntroductionSequence(introSequence);

                await accountRepository.CommitChangesAsync();
                await configurationRepository.CommitChangesAsync();

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