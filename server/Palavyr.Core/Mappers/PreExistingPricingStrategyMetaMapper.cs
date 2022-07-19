using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Mappers
{
    public class PreExistingPricingStrategyMetaMapper : IMapToPreExisting<PricingStrategyTableMetaResource, PricingStrategyTableMeta>
    {
        private readonly IUnitRetriever unitRetriever;

        public PreExistingPricingStrategyMetaMapper(IUnitRetriever unitRetriever)
        {
            this.unitRetriever = unitRetriever;
        }

        public async Task Map(PricingStrategyTableMetaResource from, PricingStrategyTableMeta to, CancellationToken cancellationToken)
        {
            await Task.Yield();
            to.UpdateProperties(from, unitRetriever);
        }
    }
}