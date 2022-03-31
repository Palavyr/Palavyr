using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCompanyLogoHandler : IRequestHandler<GetCompanyLogoRequest, GetCompanyLogoResponse>
    {
        private readonly ILogoRetriever logoRetriever;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public GetCompanyLogoHandler(
            ILogoRetriever logoRetriever,
            IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.logoRetriever = logoRetriever;
            this.mapper = mapper;
        }

        public async Task<GetCompanyLogoResponse> Handle(GetCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            var logoFileAsset = await logoRetriever.GetLogo();
            if (logoFileAsset is null) return new GetCompanyLogoResponse(new FileAssetResource());

            var fileAssetResource = await mapper.Map(logoFileAsset);

            return new GetCompanyLogoResponse(fileAssetResource);
        }
    }

    public class GetCompanyLogoResponse
    {
        public GetCompanyLogoResponse(FileAssetResource response) => Response = response;
        public FileAssetResource Response { get; set; }
    }

    public class GetCompanyLogoRequest : IRequest<GetCompanyLogoResponse>
    {
    }
}