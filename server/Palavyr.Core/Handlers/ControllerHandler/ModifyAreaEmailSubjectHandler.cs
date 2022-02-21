using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaEmailSubjectHandler : IRequestHandler<ModifyAreaEmailSubjectRequest, ModifyAreaEmailSubjectResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyAreaEmailSubjectHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyAreaEmailSubjectResponse> Handle(ModifyAreaEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var curArea = await configurationRepository.GetAreaById(request.IntentId);
            if (request.Subject != curArea.Subject)
            {
                curArea.Subject = request.Subject;
                await configurationRepository.CommitChangesAsync();
            }

            return new ModifyAreaEmailSubjectResponse(request.Subject);
        }
    }

    public class ModifyAreaEmailSubjectResponse
    {
        public ModifyAreaEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaEmailSubjectRequest : IRequest<ModifyAreaEmailSubjectResponse>
    {
        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}