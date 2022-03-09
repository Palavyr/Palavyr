using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyShowDynamicTableTotalsHandler : IRequestHandler<ModifyShowDynamicTableTotalsRequest, ModifyShowDynamicTableTotalsResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public ModifyShowDynamicTableTotalsHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyShowDynamicTableTotalsResponse> Handle(ModifyShowDynamicTableTotalsRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            area.IncludeDynamicTableTotals = request.ShowDynamicTotals;
            return new ModifyShowDynamicTableTotalsResponse(area.IncludeDynamicTableTotals);
        }
    }

    public class ModifyShowDynamicTableTotalsResponse
    {
        public ModifyShowDynamicTableTotalsResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyShowDynamicTableTotalsRequest : IRequest<ModifyShowDynamicTableTotalsResponse>
    {
        public string IntentId { get; set; }
        public bool ShowDynamicTotals { get; set; }
    }
}