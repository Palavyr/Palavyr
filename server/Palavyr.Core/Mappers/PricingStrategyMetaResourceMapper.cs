using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Mappers
{
    public class PricingStrategyMetaResourceMapper : IMapToNew<DynamicTableMeta, DynamicTableMetaResource>
    {
        public async Task<DynamicTableMetaResource> Map(DynamicTableMeta @from)
        {
            await Task.CompletedTask;
            return new DynamicTableMetaResource()
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