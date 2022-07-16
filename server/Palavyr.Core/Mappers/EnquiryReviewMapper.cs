using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Mappers
{
    public class EnquiryReviewMapper : IMapToNew<ConversationHistoryMeta, EnquiryResource>
    {
        private readonly ILinkCreator linkCreator;

        public EnquiryReviewMapper(ILinkCreator linkCreator)
        {
            this.linkCreator = linkCreator;
        }

        public async Task<EnquiryResource> Map(ConversationHistoryMeta @from, CancellationToken cancellationToken)
        {
            var fileAssetResource = new FileAssetResource();

            var fileId = @from.ResponsePdfId;
            if (!string.IsNullOrEmpty(fileId))
            {
                var link = await linkCreator.CreateLink(fileId);
                fileAssetResource.Link = link;
            }

            return new EnquiryResource
            {
                Id = @from.Id,
                ConversationId = @from.ConversationId,
                FileAssetResource = fileAssetResource,
                TimeStamp = @from.TimeStamp.ToString(),
                AccountId = @from.AccountId,
                IntentName = @from.IntentName,
                EmailTemplateUsed = @from.EmailTemplateUsed,
                Seen = @from.Seen,
                Name = @from.Name,
                Email = @from.Email,
                PhoneNumber = @from.PhoneNumber,
                HasResponse = !string.IsNullOrEmpty(fileAssetResource.Link)
            };
        }
    }
}