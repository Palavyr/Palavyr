using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers
{
    public class DeleteAttachmentHandler : IRequestHandler<DeleteAttachmentRequest, DeleteAttachmentResponse>
    {
        private readonly IAttachmentDeleter attachmentDeleter;
        private readonly IAttachmentRetriever attachmentRetriever;

        public DeleteAttachmentHandler(
            IAttachmentDeleter attachmentDeleter,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentDeleter = attachmentDeleter;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<DeleteAttachmentResponse> Handle(DeleteAttachmentRequest request, CancellationToken cancellationToken)
        {
            await attachmentDeleter.DeleteAttachment(request.FileId, cancellationToken);

            // this is currently pretty slow -- we should be caching the presigned URLs and only refreshing them once they are invalid.
            // this will always refresh the pre-signed URLs (not a huge problem, but still).
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(request.IntentId, cancellationToken);
            return new DeleteAttachmentResponse(attachmentFileLinks);
        }
    }

    public class DeleteAttachmentResponse
    {
        public DeleteAttachmentResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class DeleteAttachmentRequest : IRequest<DeleteAttachmentResponse>
    {
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}