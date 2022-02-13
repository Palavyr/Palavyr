using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Mappers
{
    public class WidgetNodeResourceMapper : IMapToNew<ConversationNode, WidgetNodeResource>
    {
        private readonly GuidFinder guidFinder;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IUnitRetriever unitRetriever;

        public WidgetNodeResourceMapper(GuidFinder guidFinder, IConfigurationRepository configurationRepository, IUnitRetriever unitRetriever)
        {
            this.guidFinder = guidFinder;
            this.configurationRepository = configurationRepository;
            this.unitRetriever = unitRetriever;
        }

        public async Task<WidgetNodeResource> Map(ConversationNode @from, CancellationToken cancellationToken)
        {
            
            
            
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
                var tableId = guidFinder.FindFirstGuidSuffix(@from.DynamicType);
                var pricingStrategyMeta = await configurationRepository.GetDynamicTableMetaByTableId(tableId);
                var unitId = pricingStrategyMeta.UnitId;
                return unitId;
                // var qauntDef = unitRetriever.GetUnitDefinitionById(unitId);
                

            }
            
            // TODO: Check if the non-pricingStrategy node is a numeric type and retrieve unitId
            // if (@from)

            return null;
        }
    }
}