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
            DynamicResponses dynamicResponses,
            CultureInfo culture,
            bool includeTotals
        )
        {
            var tableRows = new List<TableRow>();
            foreach (var dynamicResponse in dynamicResponses)
            {
                // var dynamicTableKeys = dynamicResponse.Keys.ToList(); // in the future, there could be multiple key values
                // // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
                // if (dynamicTableKeys.Count > 1) throw new Exception("Multiple dynamic table keys specified. This is a configuration error");
                // var dynamicTableKey = dynamicTableKeys[0];
                // var responses = dynamicResponse[dynamicTableKey];
                //
                // var dynamicTableName = dynamicTableKey.Split("-").First();
                // var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableName);
                var dynamicResponseComponents = pricingStrategyResponseComponentExtractor.ExtractDynamicTableComponents(dynamicResponse);

                // List<TableRow> rows;
                // rows = await compiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                var rows = await dynamicResponseComponents.Compiler.CompileToPdfTableRow(
                    dynamicResponseComponents.Responses,
                    dynamicResponseComponents.DynamicTableKeys,
                    culture
                );

                tableRows.AddRange(rows);
            }

            var table = new Table("Variable estimates determined by your responses", tableRows, culture, includeTotals);
            return new List<Table>() { table };
        }

        public async Task<List<NodeTypeOptionResource>> CompileTablesToConfigurationNodes(IEnumerable<PricingStrategyTableMeta> dynamicTableMetas, string areaId)
        {
            var nodes = new List<NodeTypeOptionResource>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
            }

            return nodes;
        }

        // public async Task<List<PricingStrategyValidationResult>> ValidatePricingStrategies(List<DynamicTableMeta> pricingStrategyMetas)
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