using Palavyr.Core.Mappers;

namespace Palavyr.Core.Resources
{
    public class EnquiryResource : IEntityResource
    {
        public int Id { get; set; }
        public string ConversationId { get; set; }
        public FileAssetResource FileAssetResource { get; set; }
        public string TimeStamp { get; set; }
        public string IntentName { get; set; }
        public string EmailTemplateUsed { get; set; }
        public bool Seen { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasResponse { get; set; }
    }
}

