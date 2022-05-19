#nullable enable
using System.Collections.Generic;

namespace Palavyr.Core.Requests
{
    public class PricingStrategyTable<T>
    {
        public List<T> TableData { get; set; }
        public string? TableTag { get; set; }
    }
}