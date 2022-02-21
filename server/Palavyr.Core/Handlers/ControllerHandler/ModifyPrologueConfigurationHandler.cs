using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPrologueConfigurationHandler : IRequestHandler<ModifyPrologueConfigurationRequest, ModifyPrologueConfigurationResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyPrologueConfigurationHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyPrologueConfigurationResponse> Handle(ModifyPrologueConfigurationRequest request, CancellationToken cancellationToken)
        {
            var updatedPrologue = request.Prologue;
            var intent = await configurationRepository.GetAreaById(request.IntentId);
            intent.Prologue = updatedPrologue;
            await configurationRepository.CommitChangesAsync();
            return new ModifyPrologueConfigurationResponse(updatedPrologue);
        }
    }

    public class ModifyPrologueConfigurationResponse
    {
        public ModifyPrologueConfigurationResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyPrologueConfigurationRequest : IRequest<ModifyPrologueConfigurationResponse>
    {
        public string Prologue { get; set; }
        public string IntentId { get; set; }
    }
}