using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyEpilogueConfigurationHandler : IRequestHandler<ModifyEpilogueConfigurationRequest, ModifyEpilogueConfigurationResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyEpilogueConfigurationHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyEpilogueConfigurationResponse> Handle(ModifyEpilogueConfigurationRequest request, CancellationToken cancellationToken)
        {
            var updatedEpilogue = request.Epilogue;
            var area = await configurationRepository.GetAreaById(request.IntentId);
            area.Epilogue = updatedEpilogue;
            await configurationRepository.CommitChangesAsync();
            return new ModifyEpilogueConfigurationResponse(updatedEpilogue);
        }
    }

    public class ModifyEpilogueConfigurationResponse
    {
        public ModifyEpilogueConfigurationResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyEpilogueConfigurationRequest : IRequest<ModifyEpilogueConfigurationResponse>
    {
        public string Epilogue { get; set; }
        public string IntentId { get; set; }
    }
}