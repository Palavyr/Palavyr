using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyTableCompiler
    {
        Task UpdateConversationNode<TEntity>(List<TEntity> table, string tableId, string intentId);

        Task CompileToConfigurationNodes(PricingStrategyTableMeta pricingStrategyTableMeta, List<NodeTypeOptionResource> nodes);

        Task<List<TableRow>> CompileToPdfTableRow(PricingStrategyResponseParts pricingStrategyResponseParts, List<string> pricingStrategyResponseIds, CultureInfo culture);
        Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents);

        // PricingStrategyValidationResult ValidatePricingStrategyPreSave<TEntity>(PricingStrategyTable<TEntity> pricingStrategyTable);
        // Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(PricingStrategyTableMeta pricingStrategyTableMeta);
        Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture);
    }
}