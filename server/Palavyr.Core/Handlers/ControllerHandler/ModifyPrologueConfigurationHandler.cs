using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPrologueConfigurationHandler : IRequestHandler<ModifyPrologueConfigurationRequest, ModifyPrologueConfigurationResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyPrologueConfigurationHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyPrologueConfigurationResponse> Handle(ModifyPrologueConfigurationRequest request, CancellationToken cancellationToken)
        {
            var updatedPrologue = request.Prologue;
            var intent = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            intent.Prologue = updatedPrologue;
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