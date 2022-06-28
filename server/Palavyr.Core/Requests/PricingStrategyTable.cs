
using System.Collections.Generic;

namespace Palavyr.Core.Requests
{
    public class PricingStrategyTable<TR> where TR
        : class
    {
        public List<TR> TableData { get; set; }
        public string? TableTag { get; set; }
    }
}