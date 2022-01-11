using System;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class ConversationRecordUpdate
    {
        public string ConversationId { get; set; }
        public string IntentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Locale { get; set; }
        public bool Fallback { get; set; }
        public bool IsComplete { get; set; }
        
        public ConversationRecordUpdate() { }
        
        private ConversationRecordUpdate(
            string conversationId,
            string intentId,
            string name,
            string email,
            string phone,
            string locale
        )
        {
            ConversationId = conversationId;
            IntentId = intentId;
            Name = name;
            Email = email;
            PhoneNumber = phone;
            Locale = locale;
        }

        public static ConversationRecordUpdate CreateNew(
            string conversationId,
            string areaIdentifier,
            string name,
            string email,
            string phone,
            string locale
        )
        {
            return new ConversationRecordUpdate(
                conversationId, 
                areaIdentifier,
                name,
                email,
                phone,
                locale);
        }

        public static ConversationRecord BindReceiverToSchemaType(
            string conversationId, 
            string accountId, 
            string areaName, 
            string emailTemplateUsed, 
            string name, 
            string email, 
            string phoneNumber,
            bool hasResponse,
            bool fallback,
            string areaIdentifier)
        {
            var timeStamp = DateTime.Now;

            var completedConversation = ConversationRecord.CreateNew(
                conversationId,
                hasResponse ? conversationId : "",
                timeStamp,
                accountId,
                areaName,
                emailTemplateUsed,
                false,
                name,
                email,
                phoneNumber,
                areaIdentifier);
            return completedConversation;
        }
    }
}