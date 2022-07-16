using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UploadAttachmentsHandler : IRequestHandler<UploadAttachmentsRequest, UploadAttachmentsResponse>
    {
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;
        private readonly IAttachmentAssetSaver attachmentAssetSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public UploadAttachmentsHandler(
            IMapToNew<FileAsset, FileAssetResource> mapper,
            IAttachmentAssetSaver attachmentAssetSaver,
            IAttachmentRetriever attachmentRetriever)
        {
            this.mapper = mapper;
            this.attachmentAssetSaver = attachmentAssetSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<UploadAttachmentsResponse> Handle(UploadAttachmentsRequest request, CancellationToken cancellationToken)
        {
            var fileAssets = new List<FileAsset>();
            foreach (var attachmentFile in request.Attachments)
            {
                var fileAsset = await attachmentAssetSaver.SaveFile(request.IntentId, attachmentFile);
                fileAssets.Add(fileAsset);
            }

            var resources = await mapper.MapMany(fileAssets, cancellationToken);
            return new UploadAttachmentsResponse(resources);
        }
    }

    public class UploadAttachmentsResponse
    {
        public UploadAttachmentsResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
    }

    public class UploadAttachmentsRequest : IRequest<UploadAttachmentsResponse>
    {
        public UploadAttachmentsRequest(string intentId, IList<IFormFile> attachments)
        {
            Attachments = attachments;
            IntentId = intentId;
        }

        public string IntentId { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}