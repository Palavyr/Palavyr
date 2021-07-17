using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class SelectOneFlatCompiler : BaseCompiler<SelectOneFlat>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;
        private readonly IConversationNodeUpdater nodeUpdater;

        public SelectOneFlatCompiler(
            IGenericDynamicTableRepository<SelectOneFlat> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter,
            IConversationNodeUpdater nodeUpdater
        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
            this.nodeUpdater = nodeUpdater;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId, string areaIdentifier, string accountId)
        {
            var currentSelectOneFlatUpdate = table.SelectOneFlat;

            var tableMeta = await context.DynamicTableMetas.SingleOrDefaultAsync(x => x.TableId == tableId);

            var conversationNodes = await context.ConversationNodes.Where(x => x.AreaIdentifier == areaIdentifier).ToListAsync(CancellationToken.None);
            var node = conversationNodes.SingleOrDefault(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId);

            if (node != null && currentSelectOneFlatUpdate != null)
            {
                if (tableMeta.ValuesAsPaths)
                {
                    var valueOptionsArray = currentSelectOneFlatUpdate.Select(x => x.Option).ToList();
                    if (splitter.SplitNodeChildrenString(node.NodeChildrenString).Length == valueOptionsArray.Count) return;

                    var valueOptionString = string.Join(Delimiters.ValueOptionDelimiter, valueOptionsArray);
                    // only update if the node exists in the conversation
                    node.ValueOptions = valueOptionString;
                    node.NodeComponentType = DefaultNodeTypeOptions.MultipleChoiceContinue.StringName;
                    var newChildNodes = new List<ConversationNode>();
                    for (var i = 0; i < currentSelectOneFlatUpdate.Count - 1; i++)
                    {
                        var newDefaultNode = ConversationNode.CreateDefaultNode(areaIdentifier, accountId).Single();
                        newChildNodes.Add(newDefaultNode);
                        node.AddChildId(newDefaultNode.NodeId!, splitter);
                    }

                    var nodeChildIds = splitter.SplitNodeChildrenString(node.NodeChildrenString);
                    if (nodeChildIds.Length != valueOptionsArray.Count) throw new Exception("We stuffed up.");

                    var originalChild = conversationNodes.Single(x => x.NodeId == nodeChildIds[0]);
                    originalChild.OptionPath = valueOptionsArray[0];


                    for (var i = 1; i < nodeChildIds.Length; i++)
                    {
                        var nodeChildId = nodeChildIds[i];
                        var childNode = newChildNodes.Single(x => x.NodeId == nodeChildId);
                        var optionPath = valueOptionsArray[i];
                        childNode.OptionPath = optionPath;
                    }

                    conversationNodes.AddRange(newChildNodes);
                    await nodeUpdater.UpdateConversation(accountId, areaIdentifier, conversationNodes, CancellationToken.None);
                }
                else
                {
                    node.ValueOptions = "Continue";
                    node.NodeComponentType = DefaultNodeTypeOptions.MultipleChoiceAsPath.StringName;
                    var unwantedChildIds = splitter.SplitNodeChildrenString(node.NodeChildrenString)[1..];
                    foreach (var unwantedChildId in unwantedChildIds)
                    {
                        var unwantedNode = conversationNodes.Single(x => x.NodeId == unwantedChildId);
                        conversationNodes.Remove(unwantedNode);
                    }
                    node.TruncateChildIdsAt(0, splitter);
                    var singleChild = conversationNodes.Single(x => x.NodeId == node.NodeChildrenString);
                    singleChild.OptionPath = "Continue";
                    await nodeUpdater.UpdateConversation(accountId, areaIdentifier, conversationNodes, CancellationToken.None);
                }
            }

            // do not save the context changes here. Following the unit of work pattern,we collect all changes, validate, and then save/commit..
        }

        public async Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes)
        {
            var rows = await GetTableRows(dynamicTableMeta);
            var valueOptions = rows.Select(x => x.Option).ToList();

            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                dynamicTableMeta.ValuesAsPaths ? valueOptions : new List<string>() {"Continue"},
                valueOptions,
                true,
                dynamicTableMeta.ValuesAsPaths,
                false,
                NodeTypeOption.CustomTables,
                dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                dynamicTableMeta.ValuesAsPaths ? NodeTypeCode.XI : NodeTypeCode.X,
                shouldRenderChildren: true,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var record = await Repository
                .GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var option = record.Single(tableRow => tableRow.Option == responseValue);
            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(option.TableId);

            var row = new TableRow(
                dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : option.Option,
                option.ValueMin,
                option.ValueMax,
                false,
                culture,
                option.Range);
            return new List<TableRow>() {row};
        }

        public Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents)
        {
            return Task.FromResult(false);
        }
    }
}