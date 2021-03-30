using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.DynamicTableService.Compilers
{
    public interface IDynamicTablesCompiler
    {
        Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes);

        Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponse dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture);
    }
}