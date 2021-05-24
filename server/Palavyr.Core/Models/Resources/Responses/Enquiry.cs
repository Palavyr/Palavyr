﻿namespace Palavyr.Core.Models.Resources.Responses
{
    public class Enquiry
    {
        public int? Id { get; set; }
        public string ConversationId { get; set; }
        public FileLinkReference LinkReference { get; set; }
        public string TimeStamp { get; set; }
        public string AccountId { get; set; }
        public string AreaName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasResponse { get; set; }
    }
}

