using System;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class CompleteConversation
    {
        public string ConversationId { get; set; }
        public string AreaIdentifier { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public CompleteConversation() { }
        
        private CompleteConversation(
            string conversationId,
            string areaIdentifier,
            string name,
            string email,
            string phone
        )
        {
            ConversationId = conversationId;
            AreaIdentifier = areaIdentifier;
            Name = name;
            Email = email;
            PhoneNumber = phone;
        }

        public static CompleteConversation CreateNew(
            string conversationId,
            string areaIdentifier,
            string name,
            string email,
            string phone
        )
        {
            return new CompleteConversation(
                conversationId, 
                areaIdentifier,
                name,
                email,
                phone);
        }

        public static CompletedConversation BindReceiverToSchemaType(string conversationId, string accountId, string areaName, string emailTemplateUsed, string name, string email, string phoneNumber)
        {
            var timeStamp = DateTime.Now;
            var seen = false;

            var completedConversation = CompletedConversation.CreateNew(
                conversationId,
                conversationId,
                timeStamp,
                accountId,
                areaName,
                emailTemplateUsed,
                seen,
                name,
                email,
                phoneNumber);
            return completedConversation;
        }
    }
}