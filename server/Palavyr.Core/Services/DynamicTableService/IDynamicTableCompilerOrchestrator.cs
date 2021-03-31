using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService
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