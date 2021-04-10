﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface IDynamicTablesCompiler
    {
        void UpdateConversationNode(DashContext context, DynamicTable table, string tableId);

        Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes);

        Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture);
    }
}