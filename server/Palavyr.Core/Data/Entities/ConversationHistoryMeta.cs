#nullable disable

using System;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class ConversationHistoryMeta : Entity, IHaveAccountId
    {
        public string ConversationId { get; set; } // This will be used when collecting enquiries. Then used to get the 
        public string ResponsePdfId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AccountId { get; set; }
        public string IntentName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IntentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFallback { get; set; }
        public string Locale { get; set; } // TODO: Correct This
        public bool IsComplete { get; set; }

        public ConversationHistoryMeta()
        {
        }

        public static ConversationHistoryMeta CreateDefault(string conversationId, string accountId, string intentName, string areaIdentifier)
        {
            return new ConversationHistoryMeta
            {
                ConversationId = conversationId,
                AccountId = accountId,
                IntentName = intentName,
                IntentId = areaIdentifier,
                TimeStamp = TimeUtils.CreateNewTimeStamp()
            };
        }

        public static ConversationHistoryMeta CreateNew(
            string conversationId,
            string responsePdfId,
            DateTime timeStamp,
            string accountId,
            string intentName,
            string emailTemplateUsed,
            bool seen,
            string name,
            string email,
            string phoneNumber,
            string areaIdentifier)
        {
            return new ConversationHistoryMeta
            {
                ConversationId = conversationId,
                ResponsePdfId = responsePdfId,
                TimeStamp = timeStamp,
                AccountId = accountId,
                IntentName = intentName,
                EmailTemplateUsed = emailTemplateUsed,
                Seen = seen,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                IntentId = areaIdentifier
            };
        }
    }
}