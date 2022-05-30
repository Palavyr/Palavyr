using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Mappers
{
    public class WidgetNodeResourceMapper : IMapToNew<ConversationNode, WidgetNodeResource>
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly ILinkCreator linkCreator;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IMapToNew<FileAsset, FileAssetResource> fileAssetMapper;
        private readonly IGuidFinder guidFinder;

        public WidgetNodeResourceMapper(
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            ILinkCreator linkCreator,
            IEntityStore<FileAsset> fileAssetStore,
            IMapToNew<FileAsset, FileAssetResource> fileAssetMapper,
            IGuidFinder guidFinder)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.linkCreator = linkCreator;
            this.fileAssetStore = fileAssetStore;
            this.fileAssetMapper = fileAssetMapper;
            this.guidFinder = guidFinder;
        }

        public async Task<WidgetNodeResource> Map(ConversationNode @from, CancellationToken cancellationToken)
        {
            FileAssetResource? fileAssetResource = null;
            if (!string.IsNullOrEmpty(@from.ImageId))
            {
                var fileAsset = await fileAssetStore.GetOrNull(@from.ImageId, s => s.FileId);
                if (fileAsset is null) throw new DomainException("The file Id attached to the convo node wasn't found.");
                fileAssetResource = await fileAssetMapper.Map(fileAsset);
            }
            
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
                FileAssetResource= fileAssetResource
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