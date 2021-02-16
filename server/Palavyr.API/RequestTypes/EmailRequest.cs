using System.Collections.Generic;

namespace Palavyr.API.RequestTypes
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string ConversationId { get; set; }
        public List<Dictionary<string, string>> KeyValues { get; set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> DynamicResponses { get; set; } = new List<Dictionary<string, string>>();
    }
}