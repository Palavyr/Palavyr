using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class TwoNestedCategoryResourceMapper : IMapToNew<TwoNestedCategory, TwoNestedCategoryResource>
    {
        public async Task<TwoNestedCategoryResource> Map(TwoNestedCategory @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new TwoNestedCategoryResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                AreaIdentifier = @from.AreaIdentifier,
                TableId = @from.TableId,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                RowId = @from.RowId,
                RowOrder = @from.RowOrder,
                ItemId = @from.ItemId,
                ItemOrder = @from.ItemOrder,
                ItemName = @from.ItemName,
                InnerItemName = @from.InnerItemName
            };
        }
    }
}