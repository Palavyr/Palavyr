using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyResponseComponentExtractor
    {
        PricingStrategyResponseComponents ExtractDynamicTableComponents(DynamicResponse dynamicResponse);
    }

    public class PricingStrategyResponseComponentExtractor : IPricingStrategyResponseComponentExtractor
    {
        private readonly IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever;

        public PricingStrategyResponseComponentExtractor(IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever)
        {
            this.pricingStrategyTableCompilerRetriever = pricingStrategyTableCompilerRetriever;
        }

        List<string> GetTableKeys(DynamicResponse dynamicResponse)
        {
            var dynamicTableKeys = dynamicResponse.Keys.ToList(); // in the future, there could be multiple key values
            // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
            if (dynamicTableKeys.Count > 1) throw new Exception("Multiple dynamic table keys specified. This is a configuration error");
            return dynamicTableKeys;
        }

        string GetDynamicTableName(string tableKey)
        {
            return tableKey.Split(Delimiters.DynamicTableKeyDelimiter).First();
        }
        
        public PricingStrategyResponseComponents ExtractDynamicTableComponents(DynamicResponse dynamicResponse)
        {
            var dynamicTableKeys = GetTableKeys(dynamicResponse);
            
            var responses = dynamicResponse[dynamicTableKeys.Single()];
            var dynamicTableName = GetDynamicTableName(dynamicTableKeys.Single());
            var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(dynamicTableName);

            return new PricingStrategyResponseComponents(compiler, responses, dynamicTableName, dynamicTableKeys);
        }
    }
}