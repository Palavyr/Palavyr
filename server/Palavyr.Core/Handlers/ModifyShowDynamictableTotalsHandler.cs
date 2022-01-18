using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyShowDynamicTableTotalsHandler : IRequestHandler<ModifyShowDynamicTableTotalsRequest, ModifyShowDynamicTableTotalsResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyShowDynamicTableTotalsHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyShowDynamicTableTotalsResponse> Handle(ModifyShowDynamicTableTotalsRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            area.IncludeDynamicTableTotals = request.ShowDynamicTotals;
            await configurationRepository.CommitChangesAsync();
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