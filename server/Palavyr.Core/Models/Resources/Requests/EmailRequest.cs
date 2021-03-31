using System.Collections.Generic;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string ConversationId { get; set; }
        public int NumIndividuals { get; set; }

        public List<Dictionary<string, string>> KeyValues { get; set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, DynamicResponse>> DynamicResponses { get; set; } = new List<Dictionary<string, DynamicResponse>>();
    }

    public class DynamicResponse
    {
        public List<Dictionary<string, string>> ResponseComponents { get; set; }
    }
}