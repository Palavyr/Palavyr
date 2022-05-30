using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAttachmentsHandler : IRequestHandler<GetAttachmentsRequest, GetAttachmentsResponse>
    {
        private readonly IAttachmentRetriever attachmentRetriever;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public GetAttachmentsHandler(IAttachmentRetriever attachmentRetriever, IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.attachmentRetriever = attachmentRetriever;
            this.mapper = mapper;
        }

        public async Task<GetAttachmentsResponse> Handle(GetAttachmentsRequest request, CancellationToken cancellationToken)
        {
            var fileAssets = await attachmentRetriever.GetAttachmentLinksForIntent(request.IntentId);
            var resources = await mapper.MapMany(fileAssets, cancellationToken);
            return new GetAttachmentsResponse(resources);
        }
    }

    public class GetAttachmentsResponse
    {
        public GetAttachmentsResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
    }

    public class GetAttachmentsRequest : IRequest<GetAttachmentsResponse>
    {
        public GetAttachmentsRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}