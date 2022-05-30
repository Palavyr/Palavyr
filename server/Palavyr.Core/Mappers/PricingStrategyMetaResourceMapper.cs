using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class PricingStrategyMetaResourceMapper : IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource>
    {
        public async Task<PricingStrategyTableMetaResource> Map(DynamicTableMeta @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new PricingStrategyTableMetaResource()
            {
                TableId = @from.TableId,
                TableType = @from.TableType,
                TableTag = @from.TableTag,
                AreaIdentifier = @from.AreaIdentifier,
                AccountId = @from.AccountId,
                PrettyName = @from.PrettyName,
                UnitId = @from.UnitId
            };
        }
    }
}