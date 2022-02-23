using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCompanyLogoHandler : IRequestHandler<GetCompanyLogoRequest, GetCompanyLogoResponse>
    {
        private readonly ILogoRetriever logoRetriever;

        public GetCompanyLogoHandler(
            ILogoRetriever logoRetriever
        )
        {
            this.logoRetriever = logoRetriever;
        }

        public async Task<GetCompanyLogoResponse> Handle(GetCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            var preSignedUrl = await logoRetriever.GetLogo();
            return new GetCompanyLogoResponse(preSignedUrl);
        }
    }

    public class GetCompanyLogoResponse
    {
        public GetCompanyLogoResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetCompanyLogoRequest : IRequest<GetCompanyLogoResponse>
    {
    }
}