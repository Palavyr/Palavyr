using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers
{
    public class CreateCompanyLogoHandler : IRequestHandler<CreateCompanyLogoRequest, CreateCompanyLogoResponse>
    {
        private readonly ILogoSaver logoSaver;
        private readonly ILogoDeleter logoDeleter;

        public CreateCompanyLogoHandler(
            ILogoSaver logoSaver,
            ILogoDeleter logoDeleter
        )
        {
            this.logoSaver = logoSaver;
            this.logoDeleter = logoDeleter;
        }

        public async Task<CreateCompanyLogoResponse> Handle(CreateCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            await logoDeleter.DeleteLogo();
            var preSignedUrl = await logoSaver.SaveLogo(request.File);
            return new CreateCompanyLogoResponse(preSignedUrl);
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