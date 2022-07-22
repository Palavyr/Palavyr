using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyResponseComponents
    {
        public IPricingStrategyTableCompiler Compiler { get; }
        public PricingStrategyResponseParts Responses { get; }
        public string PricingStrategyTableName { get; }
        public List<string> PricingStrategyTableKeys { get; }

        public PricingStrategyResponseComponents(IPricingStrategyTableCompiler compiler, PricingStrategyResponseParts responses, string pricingStrategyTableName, List<string> pricingStrategyTableKeys)
        {
            Compiler = compiler;
            Responses = responses;
            PricingStrategyTableName = pricingStrategyTableName;
            PricingStrategyTableKeys = pricingStrategyTableKeys;
        }
    }
}