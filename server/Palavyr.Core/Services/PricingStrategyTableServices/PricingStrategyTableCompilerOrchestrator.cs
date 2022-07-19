using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyTableCompilerOrchestrator : IPricingStrategyTableCompilerOrchestrator
    {
        private readonly IPricingStrategyResponseComponentExtractor pricingStrategyResponseComponentExtractor;
        private readonly IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever;

        public PricingStrategyTableCompilerOrchestrator(
            IPricingStrategyResponseComponentExtractor pricingStrategyResponseComponentExtractor,
            IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever
        )
        {
            this.pricingStrategyResponseComponentExtractor = pricingStrategyResponseComponentExtractor;
            this.pricingStrategyTableCompilerRetriever = pricingStrategyTableCompilerRetriever;
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents)
        {
            var result = await pricingStrategyResponseComponents.Compiler.PerformInternalCheck(node, response, pricingStrategyResponseComponents);
            return result;
        }

        public async Task<List<Table>> CompileTablesToPdfRows(
            PricingStrategyResponses pricingStrategyResponses,
            CultureInfo culture,
            bool includeTotals
        )
        {
            var tableRows = new List<TableRow>();
            foreach (var pricingStrategyResponse in pricingStrategyResponses)
            {
                // var pricingStrategyTableKeys = pricingStrategyResponse.Keys.ToList(); // in the future, there could be multiple key values
                // // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
                var pricingStrategyResponseComponents = pricingStrategyResponseComponentExtractor.ExtractPricingStrategyTableComponents(pricingStrategyResponse);

                var rows = await pricingStrategyResponseComponents.Compiler.CompileToPdfTableRow(
                    pricingStrategyResponseComponents.Responses,
                    pricingStrategyResponseComponents.PricingStrategyTableKeys,
                    culture
                );

                tableRows.AddRange(rows);
            }

            var table = new Table("Variable estimates determined by your responses", tableRows, culture, includeTotals);
            return new List<Table>() { table };
        }

        public async Task<List<NodeTypeOptionResource>> CompileTablesToConfigurationNodes(IEnumerable<PricingStrategyTableMeta> pricingStrategyTableMetas, string intentId)
        {
            var nodes = new List<NodeTypeOptionResource>() { };
            foreach (var pricingStrategyTableMeta in pricingStrategyTableMetas)
            {
                var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(pricingStrategyTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(pricingStrategyTableMeta, nodes);
            }

            return nodes;
        }

        // public async Task<List<PricingStrategyValidationResult>> ValidatePricingStrategies(List<PricingStrategyTableMeta> pricingStrategyMetas)
        // {
        //     var validationResults = new List<PricingStrategyValidationResult>();
        //     foreach (var pricingStrategy in pricingStrategyMetas)
        //     {
        //         var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(pricingStrategy.TableType);
        //         var validationResult = await compiler.ValidatePricingStrategyPostSave(pricingStrategy);
        //         validationResults.Add(validationResult);
        //     }
        //
        //     return validationResults;
        // }
    }
    
}