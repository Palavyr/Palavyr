using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateCompanyLogoHandler : IRequestHandler<CreateCompanyLogoRequest, CreateCompanyLogoResponse>
    {
        private readonly ILogoAssetSaver logoAssetSaver;
        private readonly ILogoDeleter logoDeleter;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public CreateCompanyLogoHandler(
            ILogoAssetSaver logoAssetSaver,
            ILogoDeleter logoDeleter,
            IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.logoAssetSaver = logoAssetSaver;
            this.logoDeleter = logoDeleter;
            this.mapper = mapper;
        }

        public async Task<CreateCompanyLogoResponse> Handle(CreateCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            await logoDeleter.DeleteLogo();
            var fileAsset = await logoAssetSaver.SaveFile(request.File);

            var fileAssetResource = await mapper.Map(fileAsset);

            return new CreateCompanyLogoResponse(fileAssetResource.Link);
        }
    }

    public class CreateCompanyLogoResponse
    {
        public string Response { get; set; }
        public CreateCompanyLogoResponse(string response) => Response = response;
    }

    public class CreateCompanyLogoRequest : IRequest<CreateCompanyLogoResponse>
    {
        public CreateCompanyLogoRequest()
        {
        }

        public CreateCompanyLogoRequest(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; set; }
    }
}