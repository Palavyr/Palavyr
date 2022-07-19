using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class PreCheckErrorResource
    {
        public string IntentName { get; set; }
        public List<string> Reasons { get; } = new List<string>();
    }
}