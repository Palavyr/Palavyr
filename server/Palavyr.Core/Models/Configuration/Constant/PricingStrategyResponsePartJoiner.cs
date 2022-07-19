using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public static class PricingStrategyResponsePartJoiner
    {
        public static PricingStrategyResponseParts CreatePricingStrategyResponseParts(string nodeId, string responseValue)
        {
            return new PricingStrategyResponseParts
            {
                new PricingStrategyResponsePart() { { nodeId, responseValue } }
            };
        }
    }
}