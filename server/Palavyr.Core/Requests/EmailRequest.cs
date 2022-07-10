using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Requests
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string? ConversationId { get; set; }
        public int NumIndividuals { get; set; }

        public List<Dictionary<string, string>> KeyValues { get; set; } = new List<Dictionary<string, string>>();
        public DynamicResponses DynamicResponses { get; set; } = new DynamicResponses();
    }
}