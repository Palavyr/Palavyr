using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateCompanyLogoHandler : INotificationHandler<LinkFileAssetToLogoRequest>
    {
        private readonly ILogoAssetSaver logoAssetSaver;
        private readonly IFileAssetLinker<LogoLinker> linker;

        public CreateCompanyLogoHandler(
            IFileAssetLinker<LogoLinker> linker)
        {
            this.linker = linker;
        }

        public async Task Handle(LinkFileAssetToLogoRequest request, CancellationToken cancellationToken)
        {
            await linker.Link(request.FileId);
        }
    }

    public class LinkFileAssetToLogoRequest : INotification
    {
        public LinkFileAssetToLogoRequest()
        {
        }

        public LinkFileAssetToLogoRequest(string fileId)
        {
            FileId = fileId;
        }

        public string FileId { get; set; }
    }
}