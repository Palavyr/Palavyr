using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class ConversationNodeResourceMapper : IMapToNew<ConversationNode, ConversationDesignerNodeResource>
    {
        public async Task<ConversationDesignerNodeResource> Map(ConversationNode @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new ConversationDesignerNodeResource()
            {
                NodeId = @from.NodeId,
                NodeType = @from.NodeType,
                Text = @from.Text,
                IsRoot = @from.IsRoot,
                IntentId = @from.IntentId,
                NodeChildrenString = @from.NodeChildrenString,
                OptionPath = @from.OptionPath,
                ValueOptions = @from.ValueOptions,
                IsCritical = @from.IsCritical,
                AccountId = @from.AccountId,
                IsMultiOptionType = @from.IsMultiOptionType,
                IsTerminalType = @from.IsTerminalType,
                ShouldRenderChildren = @from.ShouldRenderChildren,
                ShouldShowMultiOption = @from.ShouldShowMultiOption,
                IsAnabranchType = @from.IsAnabranchType,
                IsAnabranchMergePoint = @from.IsAnabranchMergePoint,
                IsCurrency = @from.IsCurrency,
                IsMultiOptionEditable = @from.IsMultiOptionEditable,
                IsDynamicTableNode = @from.IsDynamicTableNode,
                ResolveOrder = @from.ResolveOrder,
                NodeComponentType = @from.NodeComponentType,
                DynamicType = @from.DynamicType,
                IsImageNode = @from.IsImageNode,
                ImageId = @from.ImageId,
                IsLoopbackAnchorType = @from.IsLoopbackAnchorType,
                NodeTypeCode = @from.NodeTypeCode
            };
        }
    }
}