using System.Collections.Generic;

namespace Palavyr.API.RequestTypes
{
    public class EmailRequest
    {
        public string EmailAddress { get; set; }
        public string ConversationId { get; set; }
        public List<Dictionary<string, string>> KeyValues { get; set; }
        public List<Dictionary<string, string>> DynamicResponses { get; set; }
    }
}