using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetIntroductionSequenceHandler : IRequestHandler<GetIntroductionSequenceRequest, GetIntroductionSequenceResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;

        public GetIntroductionSequenceHandler(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository)
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<GetIntroductionSequenceResponse> Handle(GetIntroductionSequenceRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            var introConvo = await configurationRepository.GetIntroductionSequence(account.IntroductionId);
            var intro = EndingSequence.CleanTheIntroConvoEnding(introConvo);
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