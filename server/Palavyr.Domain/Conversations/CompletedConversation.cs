using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.conversations
{
    public class CompletedConversation
    {
        [Key]
        public int Id { get; set; }
        public string ConversationId { get; set; }
        public string ResponsePdfId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AccountId { get; set; }
        public string AreaName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static CompletedConversation CreateNew(string conversationId, string responsePdfId, DateTime timeStamp,
            string accountId, string areaName, string emailTemplateUsed, bool seen, string name, string email, string phoneNumber)
        {
            return new CompletedConversation()
            {
                ConversationId = conversationId,
                ResponsePdfId = responsePdfId,
                TimeStamp = timeStamp,
                AccountId = accountId,
                AreaName = areaName,
                EmailTemplateUsed = emailTemplateUsed,
                Seen = seen,
                Name = name, 
                Email = email,
                PhoneNumber = phoneNumber
            };
        }
        
    }
}