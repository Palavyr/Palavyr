using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyIntentIsCompleteHandler : IRequestHandler<ModifyIntentIsCompleteRequest, ModifyIntentIsCompleteResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyIntentIsCompleteHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyIntentIsCompleteResponse> Handle(ModifyIntentIsCompleteRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            area.IsEnabled = request.IsEnabled;
            await configurationRepository.CommitChangesAsync();
            return new ModifyIntentIsCompleteResponse(area.IsEnabled);
        }
    }

    public class ModifyIntentIsCompleteResponse
    {
        public ModifyIntentIsCompleteResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyIntentIsCompleteRequest : IRequest<ModifyIntentIsCompleteResponse>
    {
        public string IntentId { get; set; }
        public bool IsEnabled { get; set; }
    }
}