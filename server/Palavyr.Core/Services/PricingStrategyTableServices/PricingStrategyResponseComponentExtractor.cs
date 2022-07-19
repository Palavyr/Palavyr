using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyResponseComponentExtractor
    {
        PricingStrategyResponseComponents ExtractPricingStrategyTableComponents(PricingStrategyResponse pricingStrategyResponse);
    }

    public class PricingStrategyResponseComponentExtractor : IPricingStrategyResponseComponentExtractor
    {
        private readonly IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever;

        public PricingStrategyResponseComponentExtractor(IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever)
        {
            this.pricingStrategyTableCompilerRetriever = pricingStrategyTableCompilerRetriever;
        }

        List<string> GetTableKeys(PricingStrategyResponse pricingStrategyResponse)
        {
            var pricingStrategyTableKey = pricingStrategyResponse.Keys.ToList(); // in the future, there could be multiple key values
            // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
            if (pricingStrategyTableKey.Count > 1) throw new Exception("Multiple pricing strategy table keys specified. This is a configuration error");
            return pricingStrategyTableKey;
        }

        string GetPricingStrategyTableName(string tableKey)
        {
            return tableKey.Split(Delimiters.PricingStrategyTableKeyDelimiter).First();
        }
        
        public PricingStrategyResponseComponents ExtractPricingStrategyTableComponents(PricingStrategyResponse pricingStrategyResponse)
        {
            var pricingStrategyTableKeys = GetTableKeys(pricingStrategyResponse);
            
            var responses = pricingStrategyResponse[pricingStrategyTableKeys.Single()];
            var pricingStrategyTableName = GetPricingStrategyTableName(pricingStrategyTableKeys.Single());
            var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(pricingStrategyTableName);

            return new PricingStrategyResponseComponents(compiler, responses, pricingStrategyTableName, pricingStrategyTableKeys);
        }
    }
}