using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Mappers
{
    public class WidgetNodeResourceMapper : IMapToNew<ConversationNode, WidgetNodeResource>
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly IGuidFinder guidFinder;

        public WidgetNodeResourceMapper(
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            IGuidFinder guidFinder)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.guidFinder = guidFinder;
        }

        public async Task<WidgetNodeResource> Map(ConversationNode @from)
        {
            await Task.CompletedTask;
            return new WidgetNodeResource
            {
                AreaIdentifier = @from.AreaIdentifier,
                Text = @from.Text,
                NodeType = @from.NodeType,
                NodeComponentType = @from.NodeComponentType,
                NodeId = @from.NodeId,
                NodeChildrenString = @from.NodeChildrenString,
                IsRoot = @from.IsRoot,
                IsCritical = @from.IsCritical,
                OptionPath = @from.OptionPath,
                ValueOptions = @from.ValueOptions,
                IsDynamicTableNode = @from.IsDynamicTableNode,
                DynamicType = @from.DynamicType,
                ResolveOrder = @from.ResolveOrder,
                // UnitId = await AttachUnitIdOrNull(@from)
            };
        }

        private async Task<UnitIds?> AttachUnitIdOrNull(ConversationNode @from)
        {
            if (@from.IsDynamicTableNode)
            {
                if (@from.DynamicType == null) throw new DomainException("Dynamic Type not set.");
                var tableId = guidFinder.FindFirstGuidSuffixOrNull(@from.DynamicType);

                var pricingStrategyMeta = await dynamicTableMetaStore.Get(tableId, s => s.TableId);
                var unitId = pricingStrategyMeta.UnitId;
                return unitId;
            }

            // TODO: Check if the non-pricingStrategy node is a numeric type and retrieve unitId
            // if (@from)

            return null;
        }
    }
}