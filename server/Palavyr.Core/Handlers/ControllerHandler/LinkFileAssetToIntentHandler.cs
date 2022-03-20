using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class LinkFileAssetToIntentHandler : INotificationHandler<LinkFileAssetToIntentRequest>
    {
        private readonly IFileAssetLinker<AttachmentLinker> linker;

        public LinkFileAssetToIntentHandler(IFileAssetLinker<AttachmentLinker> linker)
        {
            this.linker = linker;
        }

        public async Task Handle(LinkFileAssetToIntentRequest request, CancellationToken cancellationToken)
        {
            // asserts this image exists
            await linker.Link(request.FileId, request.IntentId);
        }
    }


    public class LinkFileAssetToIntentRequest : INotification
    {
        public LinkFileAssetToIntentRequest(string fileId, string intentId)
        {
            FileId = fileId;
            IntentId = intentId;
        }

        public string FileId { get; set; }
        public string IntentId { get; set; }
    }
}