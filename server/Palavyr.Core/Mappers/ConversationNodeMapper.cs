using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers
{
    public class ConversationNodeMapper : IMapToNew<ConversationDesignerNodeResource, ConversationNode>
    {
        private readonly IAccountIdTransport accountIdTransport;

        // TODO: This should not be the way -- IMapToExisting should be used here
        public ConversationNodeMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<ConversationNode> Map(ConversationDesignerNodeResource @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return ConversationNode.CreateNew(
                @from.NodeId,
                @from.NodeType,
                @from.Text,
                @from.AreaIdentifier,
                @from.NodeChildrenString,
                @from.OptionPath,
                @from.ValueOptions,
                accountIdTransport.AccountId,
                @from.NodeComponentType,
                @from.NodeTypeCode,
                @from.IsRoot,
                @from.IsCritical,
                @from.IsMultiOptionType,
                @from.IsTerminalType,
                @from.ShouldRenderChildren,
                @from.ShouldShowMultiOption,
                @from.IsAnabranchType,
                @from.IsAnabranchMergePoint,
                @from.IsDynamicTableNode,
                @from.IsCurrency,
                @from.IsMultiOptionEditable,
                @from.ResolveOrder,
                @from.DynamicType,
                @from.IsImageNode,
                @from.ImageId,
                @from.IsLoopbackAnchorType
            );
        }
    }
}