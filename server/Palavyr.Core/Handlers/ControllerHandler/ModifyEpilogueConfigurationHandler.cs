using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyEpilogueConfigurationHandler : IRequestHandler<ModifyEpilogueConfigurationRequest, ModifyEpilogueConfigurationResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyEpilogueConfigurationHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyEpilogueConfigurationResponse> Handle(ModifyEpilogueConfigurationRequest request, CancellationToken cancellationToken)
        {
            var updatedEpilogue = request.Epilogue;
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            area.Epilogue = updatedEpilogue;
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