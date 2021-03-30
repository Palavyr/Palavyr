using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.DynamicTableService
{
    public interface IDynamicTableCompilerOrchestrator
    {
        Task<List<Table>> CompileTablesToPdfRows(
            string accountId,
            List<Dictionary<string, DynamicResponse>> dynamicResponses,
            CultureInfo culture
        );

        Task<List<NodeTypeOption>> CompileTablesToConfigurationNodes(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        );
    }
}