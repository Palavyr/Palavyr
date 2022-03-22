using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Mappers
{
    public class EnquiryReviewMapper : IMapToNew<ConversationRecord, Enquiry>
    {
        private readonly ILinkCreator linkCreator;

        public EnquiryReviewMapper(ILinkCreator linkCreator)
        {
            this.linkCreator = linkCreator;
        }

        public async Task<Enquiry> Map(ConversationRecord @from)
        {
            var fileAssetResource = new FileAssetResource();

            var fileId = @from.ResponsePdfId;
            if (!string.IsNullOrEmpty(fileId))
            {
                var link = await linkCreator.CreateLink(fileId);
                fileAssetResource.Link = link;
            }

            return new Enquiry
            {
                Id = @from.Id,
                ConversationId = @from.ConversationId,
                FileAssetResource = fileAssetResource,
                TimeStamp = @from.TimeStamp.ToString(),
                AccountId = @from.AccountId,
                AreaName = @from.AreaName,
                EmailTemplateUsed = @from.EmailTemplateUsed,
                Seen = @from.Seen,
                Name = @from.Name,
                Email = @from.Email,
                PhoneNumber = @from.PhoneNumber,
                HasResponse = !string.IsNullOrEmpty(fileId)
            };
        }
    }
}