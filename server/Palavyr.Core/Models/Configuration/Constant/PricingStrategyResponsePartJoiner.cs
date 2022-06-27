using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public static class PricingStrategyResponsePartJoiner
    {
        public static DynamicResponseParts CreateDynamicResponseParts(string nodeId, string responseValue)
        {
            return new DynamicResponseParts
            {
                new DynamicResponsePart() { { nodeId, responseValue } }
            };
        }
    }
}