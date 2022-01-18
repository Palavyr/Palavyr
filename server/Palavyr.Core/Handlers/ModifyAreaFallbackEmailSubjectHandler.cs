using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyAreaFallbackEmailSubjectHandler : IRequestHandler<ModifyAreaFallbackEmailSubjectRequest, ModifyAreaFallbackEmailSubjectResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyAreaFallbackEmailSubjectHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyAreaFallbackEmailSubjectResponse> Handle(ModifyAreaFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var curArea = await configurationRepository.GetAreaById(request.IntentId);

            if (request.Subject != curArea.FallbackSubject)
            {
                curArea.FallbackSubject = request.Subject;
                await configurationRepository.CommitChangesAsync();
            }

            return new ModifyAreaFallbackEmailSubjectResponse(curArea.FallbackSubject);
        }
    }

    public class ModifyAreaFallbackEmailSubjectResponse
    {
        public ModifyAreaFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaFallbackEmailSubjectRequest : IRequest<ModifyAreaFallbackEmailSubjectResponse>
    {
        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}