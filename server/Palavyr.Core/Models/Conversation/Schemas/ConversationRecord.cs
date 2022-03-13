using System;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.Core.Models.Conversation.Schemas
{
    public class ConversationRecord : Entity, IHaveAccountId
    {
        public string ConversationId { get; set; } // This will be used when collecting enquiries. Then used to get the 
        public string ResponsePdfId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AccountId { get; set; }
        public string AreaName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AreaIdentifier { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFallback { get; set; }
        public string Locale { get; set; } // TODO: Correct This
        public bool IsComplete { get; set; }
        
        public static ConversationRecord CreateDefault(string conversationId, string accountId, string areaName, string areaIdentifier)
        {
            return new ConversationRecord
            {
                ConversationId = conversationId,
                AccountId = accountId,
                AreaName = areaName,
                AreaIdentifier = areaIdentifier,
                TimeStamp = TimeUtils.CreateNewTimeStamp()
            };
        }

        public ConversationRecord ApplyEmailRequest(EmailRequest request)
        {
            Name = request.Name;
            Email = request.EmailAddress;
            PhoneNumber = request.Phone;
            return this;
        }

        public static ConversationRecord CreateNew(
            string conversationId,
            string responsePdfId,
            DateTime timeStamp,
            string accountId,
            string areaName,
            string emailTemplateUsed,
            bool seen,
            string name,
            string email,
            string phoneNumber,
            string areaIdentifier)
        {
            return new ConversationRecord
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
                PhoneNumber = phoneNumber,
                AreaIdentifier = areaIdentifier
            };
        }
    }
}