using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class TwoNestedCategoryMapper : IMapToNew<TwoNestedCategoryResource, TwoNestedSelectTableRow>
    {
        public async Task<TwoNestedSelectTableRow> Map(TwoNestedCategoryResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new TwoNestedSelectTableRow
            {
                Id = from.Id,
                Range = from.Range,
                AccountId = from.AccountId,
                AreaIdentifier = from.AreaIdentifier,
                ItemId = from.ItemId,
                ItemName = from.ItemId,
                ItemOrder = from.ItemOrder,
                RowId = from.RowId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin,
                InnerItemName = from.InnerItemName
            };
        }
    }
}