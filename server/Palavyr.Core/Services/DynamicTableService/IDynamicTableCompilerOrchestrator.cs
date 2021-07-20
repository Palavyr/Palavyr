using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService
{
    public interface IDynamicTableCompilerOrchestrator
    {
        Task<List<Table>> CompileTablesToPdfRows(
            string accountId,
            DynamicResponses dynamicResponses,
            CultureInfo culture
        );

        Task<List<NodeTypeOption>> CompileTablesToConfigurationNodes(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        );

        Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents);
        Task<List<PricingStrategyValidationResult>> ValidatePricingStrategies(List<DynamicTableMeta> pricingStrategyMetas);
    }
}