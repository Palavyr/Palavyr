using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface IDynamicTablesCompiler
    {
        Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId);

        Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes);

        Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture);
        Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents);
    }
}