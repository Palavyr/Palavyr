using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Mappers
{
    public class WidgetNodeResourceMapper : IMapToNew<ConversationNode, WidgetNodeResource>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IMapToNew<FileAsset, FileAssetResource> fileAssetMapper;
        private readonly IGuidFinder guidFinder;

        public WidgetNodeResourceMapper(
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore,
            IEntityStore<FileAsset> fileAssetStore,
            IMapToNew<FileAsset, FileAssetResource> fileAssetMapper,
            IGuidFinder guidFinder)
        {
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.fileAssetStore = fileAssetStore;
            this.fileAssetMapper = fileAssetMapper;
            this.guidFinder = guidFinder;
        }

        public async Task<WidgetNodeResource> Map(ConversationNode @from, CancellationToken cancellationToken)
        {
            FileAssetResource? fileAssetResource = null;
            if (!string.IsNullOrEmpty(@from.FileId))
            {
                var fileAsset = await fileAssetStore.GetOrNull(@from.FileId, s => s.FileId);
                if (fileAsset is null) throw new DomainException("The file Id attached to the convo node wasn't found.");
                fileAssetResource = await fileAssetMapper.Map(fileAsset);
            }
            
            return new WidgetNodeResource
            {
                IntentId = @from.IntentId,
                Text = @from.Text,
                NodeType = @from.NodeType,
                NodeComponentType = @from.NodeComponentType,
                NodeId = @from.NodeId,
                NodeChildrenString = @from.NodeChildrenString,
                IsRoot = @from.IsRoot,
                IsCritical = @from.IsCritical,
                OptionPath = @from.OptionPath,
                ValueOptions = @from.ValueOptions,
                IsPricingStrategyTableNode = @from.IsPricingStrategyTableNode,
                PricingStrategyType = @from.PricingStrategyType,
                ResolveOrder = @from.ResolveOrder,
                FileAssetResource= fileAssetResource
                // UnitId = await AttachUnitIdOrNull(@from)
            };
        }

        private async Task<UnitIdEnum?> AttachUnitIdOrNull(ConversationNode @from)
        {
            if (@from.IsPricingStrategyTableNode)
            {
                if (@from.PricingStrategyType == null) throw new DomainException("Pricing Strategy Type not set.");
                var tableId = guidFinder.FindFirstGuidSuffixOrNull(@from.PricingStrategyType);

                var pricingStrategyMeta = await pricingStrategyTableMetaStore.Get(tableId, s => s.TableId);
                var unitId = pricingStrategyMeta.UnitIdEnum;
                return unitId;
            }

            // TODO: Check if the non-pricingStrategy node is a numeric type and retrieve unitId
            // if (@from)

            return null;
        }
    }
}