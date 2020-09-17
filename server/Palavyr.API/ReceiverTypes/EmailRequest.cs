using System.Collections.Generic;

namespace Palavyr.API.receiverTypes
{
    public class EmailRequest
    {
        public string EmailAddress { get; set; }
        public List<Dictionary<string, string>> KeyValues { get; set; }
        public List<Dictionary<string, string>> DynamicResponse { get; set; }
    }
}