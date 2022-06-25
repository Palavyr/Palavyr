using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyTableCompiler
    {
        Task UpdateConversationNode<TEntity>(List<TEntity> table, string tableId, string areaIdentifier);

        Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOptionResource> nodes);

        Task<List<TableRow>> CompileToPdfTableRow(DynamicResponseParts dynamicResponseParts, List<string> dynamicResponseIds, CultureInfo culture);
        Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents);

        // PricingStrategyValidationResult ValidatePricingStrategyPreSave<TEntity>(PricingStrategyTable<TEntity> pricingStrategyTable);
        // Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta);
        Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area area, CultureInfo culture);
    }
}