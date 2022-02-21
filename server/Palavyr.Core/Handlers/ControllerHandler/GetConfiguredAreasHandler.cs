using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConfiguredIntentsHandler : IRequestHandler<GetConfiguredIntentsRequest, GetConfiguredIntentsResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetConfiguredIntentsHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetConfiguredIntentsResponse> Handle(GetConfiguredIntentsRequest request, CancellationToken cancellationToken)
        {
            var activeIntents = await configurationRepository.GetActiveAreas();
            return new GetConfiguredIntentsResponse(activeIntents);
        }
    }

    public class GetConfiguredIntentsResponse
    {
        public GetConfiguredIntentsResponse(List<Area> response) => Response = response;
        public List<Area> Response { get; set; }
    }

    public class GetConfiguredIntentsRequest : IRequest<GetConfiguredIntentsResponse>
    {
    }
}