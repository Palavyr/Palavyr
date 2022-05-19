using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyResponseComponents
    {
        public IPricingStrategyTableCompiler Compiler { get; }
        public DynamicResponseParts Responses { get; }
        public string DynamicTableName { get; }
        public List<string> DynamicTableKeys { get; }

        public PricingStrategyResponseComponents(IPricingStrategyTableCompiler compiler, DynamicResponseParts responses, string dynamicTableName, List<string> dynamicTableKeys)
        {
            Compiler = compiler;
            Responses = responses;
            DynamicTableName = dynamicTableName;
            DynamicTableKeys = dynamicTableKeys;
        }
    }
}