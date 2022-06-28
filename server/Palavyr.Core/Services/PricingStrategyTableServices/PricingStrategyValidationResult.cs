
using System.Collections.Generic;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyValidationResult
    {
        public string TableName { get; private set; }
        public bool IsValid { get; private set; }

        public List<string>? Reasons { get; private set; }

        private PricingStrategyValidationResult(string name, bool isValid, List<string>? reasons)
        {
            TableName = name;
            IsValid = isValid;
            Reasons = reasons;
        }

        public static PricingStrategyValidationResult CreateValid(string name)
        {
            return new PricingStrategyValidationResult(name, true, null);
        }

        public static PricingStrategyValidationResult CreateInvalid(string name, List<string> reasons)
        {
            return new PricingStrategyValidationResult(name, false, reasons);
        }
    }
}