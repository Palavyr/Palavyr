using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyTableCompilerOrchestrator
    {
        Task<List<Table>> CompileTablesToPdfRows(
            DynamicResponses dynamicResponses,
            CultureInfo culture,
            bool includeTotals
        );

        Task<List<NodeTypeOptionResource>> CompileTablesToConfigurationNodes(IEnumerable<PricingStrategyTableMeta> dynamicTableMetas, string areaId);

        Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents);
        // Task<List<PricingStrategyValidationResult>> ValidatePricingStrategies(List<DynamicTableMeta> pricingStrategyMetas);
    }
}