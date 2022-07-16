using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.MapToExisting
{
    public class SelectOneFlatToExistingMapper : IMapToPreExisting<SelectOneFlatResource, CategorySelectTableRow>
    {
        public async Task Map(SelectOneFlatResource from, CategorySelectTableRow to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            to.Id = from.Id;
            to.Category = from.Option;
            to.Range = from.Range;
            to.AccountId = from.AccountId;
            to.IntentId = from.AreaIdentifier;
            to.RowOrder = from.RowOrder;
            to.TableId = from.TableId;
            to.ValueMax = from.ValueMax;
            to.ValueMin = from.ValueMin;
        }
    }

    public class BasicThresholdToExistingMapper : IMapToPreExisting<BasicThresholdResource, SimpleThresholdTableRow>
    {
        public async Task Map(BasicThresholdResource from, SimpleThresholdTableRow to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            to.Id = from.Id;
            to.Range = from.Range;
            to.Threshold = from.Threshold;
            to.AccountId = from.AccountId;
            to.IntentId = from.AreaIdentifier;
            to.ItemName = from.ItemName;
            to.RowId = from.RowId;
            to.RowOrder = from.RowOrder;
            to.TableId = from.TableId;
            to.TriggerFallback = from.TriggerFallback;
            to.ValueMax = from.ValueMax;
            to.ValueMin = from.ValueMin;
        }
    }

    public class CategoryNestedThresholdToExistingMapper : IMapToPreExisting<CategoryNestedThresholdResource, CategoryNestedThresholdTableRow>
    {
        public async Task Map(CategoryNestedThresholdResource from, CategoryNestedThresholdTableRow to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            to.Id = from.Id;
            to.Range = from.Range;
            to.Threshold = from.Threshold;
            to.AccountId = from.AccountId;
            to.IntentId = from.AreaIdentifier;
            to.ItemId = from.ItemId;
            to.Category = from.ItemName;
            to.ItemOrder = from.ItemOrder;
            to.RowId = from.RowId;
            to.RowOrder = from.RowOrder;
            to.TableId = from.TableId;
            to.TriggerFallback = from.TriggerFallback;
            to.ValueMax = from.ValueMax;
            to.ValueMin = from.ValueMin;
        }
    }

    public class PercentOfThresholdToExistingMapper : IMapToPreExisting<PercentOfThresholdResource, PercentOfThresholdTableRow>
    {
        public async Task Map(PercentOfThresholdResource from, PercentOfThresholdTableRow to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            to.Id = from.Id;
            to.Modifier = from.Modifier;
            to.Range = from.Range;
            to.Threshold = from.Threshold;
            to.AccountId = from.AccountId;
            to.IntentId = from.AreaIdentifier;
            to.ItemId = from.ItemId;
            to.Category = from.ItemName;
            to.ItemOrder = from.ItemOrder;
            to.PosNeg = from.PosNeg;
            to.RowId = from.RowId;
            to.RowOrder = from.RowOrder;
            to.TableId = from.TableId;
            to.TriggerFallback = from.TriggerFallback;
            to.ValueMax = from.ValueMax;
            to.ValueMin = from.ValueMin;
        }
    }

    public class TwoNestedCategoryToExistingMapper : IMapToPreExisting<TwoNestedCategoryResource, TwoNestedSelectTableRow>
    {
        public async Task Map(TwoNestedCategoryResource from, TwoNestedSelectTableRow to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            to.Id = from.Id;
            to.Range = from.Range;
            to.AccountId = from.AccountId;
            to.IntentId = from.AreaIdentifier;
            to.ItemId = from.ItemId;
            to.Category = from.ItemId;
            to.ItemOrder = from.ItemOrder;
            to.RowId = from.RowId;
            to.RowOrder = from.RowOrder;
            to.TableId = from.TableId;
            to.ValueMax = from.ValueMax;
            to.ValueMin = from.ValueMin;
            to.InnerItemName = from.InnerItemName;
        }
    }
}