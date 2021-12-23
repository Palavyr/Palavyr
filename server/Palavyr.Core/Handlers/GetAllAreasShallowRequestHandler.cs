using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetAllAreasShallowRequestHandler : IRequestHandler<GetAllAreasRequest, GetAllAreasResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<GetAllAreasShallowRequestHandler> logger;

        public GetAllAreasShallowRequestHandler(IConfigurationRepository configurationRepository, ILogger<GetAllAreasShallowRequestHandler> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        public async Task<GetAllAreasResponse> Handle(GetAllAreasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Return all areas");
            var areas = await configurationRepository.GetAllAreasShallow();
            return new GetAllAreasResponse(areas);
        }
    }

    public class GetAllAreasRequest : IRequest<GetAllAreasResponse>
    {
    }

    public class GetAllAreasResponse
    {
        public List<Area> AllAreasShallow { get; }

        public GetAllAreasResponse(List<Area> allAreasShallow)
        {
            AllAreasShallow = allAreasShallow;
        }
    }
}