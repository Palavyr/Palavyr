using System.Collections.Generic;

namespace Palavyr.Core.Models
{
    public class NodeOrderCheckResult
    {
        public bool IsOrdered { get; set; }
        public List<string> ConcatenatedNodeTypes { get; set; }
    }
}