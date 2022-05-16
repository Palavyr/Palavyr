#nullable enable
using System.Collections.Generic;

namespace Palavyr.Core.Requests
{
    public class DynamicTable<T>
    {
        public List<T> TableData { get; set; }
        public string? TableTag { get; set; }
    }
}