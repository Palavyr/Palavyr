using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetResponseConfigurationHandler : IRequestHandler<GetResponseConfigurationRequest, GetResponseConfigurationResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetResponseConfigurationHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetResponseConfigurationResponse> Handle(GetResponseConfigurationRequest request, CancellationToken cancellationToken)
        {
            var areaWithAllData = await configurationRepository.GetAreaComplete(request.IntentId);
            return new GetResponseConfigurationResponse(areaWithAllData);
        }
    }

    public class GetResponseConfigurationResponse
    {
        public GetResponseConfigurationResponse(Area response) => Response = response;
        public Area Response { get; set; }
    }

    public class GetResponseConfigurationRequest : IRequest<GetResponseConfigurationResponse>
    {
        public GetResponseConfigurationRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}