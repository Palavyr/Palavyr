using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteAttachmentHandler : IRequestHandler<DeleteAttachmentRequest, DeleteAttachmentResponse>
    {
        private readonly IAttachmentDeleter attachmentDeleter;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;
        private readonly IAttachmentRetriever attachmentRetriever;

        public DeleteAttachmentHandler(
            IAttachmentDeleter attachmentDeleter,
            IMapToNew<FileAsset, FileAssetResource> mapper,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentDeleter = attachmentDeleter;
            this.mapper = mapper;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<DeleteAttachmentResponse> Handle(DeleteAttachmentRequest request, CancellationToken cancellationToken)
        {
            await attachmentDeleter.DeleteAttachment(request.FileId, request.IntentId);
            var fileAssets = await attachmentRetriever.GetAttachmentLinksForIntent(request.IntentId);

            var resources = await mapper.MapMany(fileAssets, cancellationToken);
            return new DeleteAttachmentResponse(resources);
        }
    }

    public class DeleteAttachmentResponse
    {
        public DeleteAttachmentResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
    }

    public class DeleteAttachmentRequest : IRequest<DeleteAttachmentResponse>
    {
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}