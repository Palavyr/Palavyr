using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetImageFileLinkHandler : IRequestHandler<GetImageFileLinkRequest, GetImageFileLinkResponse>
    {
        private readonly IConfigurationEntityStore<FileAsset> fileAssetStore;
        private readonly ILinkCreator linkCreator;

        public GetImageFileLinkHandler(
            IConfigurationEntityStore<FileAsset> fileAssetStore,
            ILinkCreator linkCreator)
        {
            this.fileAssetStore = fileAssetStore;
            this.linkCreator = linkCreator;
        }

        public async Task<GetImageFileLinkResponse> Handle(GetImageFileLinkRequest request, CancellationToken cancellationToken)
        {
            var imageAsset = await fileAssetStore.Get(request.FileId, x => x.FileId);
            var link = await linkCreator.CreateLink(imageAsset.FileId);
            return new GetImageFileLinkResponse(link);
        }
    }

    public class GetImageFileLinkResponse
    {
        public GetImageFileLinkResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetImageFileLinkRequest : IRequest<GetImageFileLinkResponse>
    {
        public string FileId { get; set; }
        // public string S3Key { get; set; }
    }
}