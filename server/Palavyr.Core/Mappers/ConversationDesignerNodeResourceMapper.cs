using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class ConversationDesignerNodeResourceMapper : IMapToNew<ConversationNode, ConversationDesignerNodeResource>
    {
        public async Task<ConversationDesignerNodeResource> Map(ConversationNode @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new ConversationDesignerNodeResource
            {
                Id = @from.Id,
                NodeId = @from.NodeId,
                NodeType = @from.NodeType,
                Text = @from.Text,
                IsRoot = @from.IsRoot,
                IntentId = @from.IntentId,
                NodeChildrenString = @from.NodeChildrenString,
                OptionPath = @from.OptionPath,
                ValueOptions = @from.ValueOptions,
                IsCritical = @from.IsCritical,
                IsMultiOptionType = @from.IsMultiOptionType,
                IsTerminalType = @from.IsTerminalType,
                ShouldRenderChildren = @from.ShouldRenderChildren,
                ShouldShowMultiOption = @from.ShouldShowMultiOption,
                IsAnabranchType = @from.IsAnabranchType,
                IsAnabranchMergePoint = @from.IsAnabranchMergePoint,
                IsCurrency = @from.IsCurrency,
                IsMultiOptionEditable = @from.IsMultiOptionEditable,
                IsPricingStrategyNode = @from.IsPricingStrategyTableNode,
                ResolveOrder = @from.ResolveOrder,
                NodeComponentType = @from.NodeComponentType,
                PricingStrategyType = @from.PricingStrategyType,
                IsImageNode = @from.IsImageNode,
                FileId = @from.FileId,
                IsLoopbackAnchorType = @from.IsLoopbackAnchorType,
                NodeTypeCodeEnum = @from.NodeTypeCodeEnum
            };
        }
    }
}