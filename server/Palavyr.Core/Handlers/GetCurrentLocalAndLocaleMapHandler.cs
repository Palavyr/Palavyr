using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Core.Handlers
{
    public class GetCurrentLocalAndLocaleMapHandler : IRequestHandler<GetCurrentLocalAndLocaleMapRequest, GetCurrentLocalAndLocaleMapResponse>
    {
        private readonly ICurrentLocaleAndLocalMapRetriever currentLocaleAndLocalMapRetriever;

        public GetCurrentLocalAndLocaleMapHandler(ICurrentLocaleAndLocalMapRetriever currentLocaleAndLocalMapRetriever)
        {
            this.currentLocaleAndLocalMapRetriever = currentLocaleAndLocalMapRetriever;
        }

        public async Task<GetCurrentLocalAndLocaleMapResponse> Handle(GetCurrentLocalAndLocaleMapRequest request, CancellationToken cancellationToken)
        {
            var localeDetails = await currentLocaleAndLocalMapRetriever.GetLocaleDetails();
            return new GetCurrentLocalAndLocaleMapResponse(localeDetails);
        }
    }

    public class GetCurrentLocalAndLocaleMapResponse
    {
        public GetCurrentLocalAndLocaleMapResponse(CurrentLocaleAndLocalMapRetriever.LocaleResponse response) => Response = response;
        public CurrentLocaleAndLocalMapRetriever.LocaleResponse Response { get; set; }
    }

    public class GetCurrentLocalAndLocaleMapRequest : IRequest<GetCurrentLocalAndLocaleMapResponse>
    {
    }
}