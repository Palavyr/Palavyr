using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetDynamicTableMetasHandler : IRequestHandler<GetDynamicTableMetasRequest, GetDynamicTableMetasResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<GetDynamicTableMetasHandler> logger;

        public GetDynamicTableMetasHandler(IConfigurationRepository configurationRepository, ILogger<GetDynamicTableMetasHandler> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        public async Task<GetDynamicTableMetasResponse> Handle(GetDynamicTableMetasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await configurationRepository.GetDynamicTableMetas(request.IntentId);
            return new GetDynamicTableMetasResponse(tableTypes.ToArray());
        }
    }

    public class GetDynamicTableMetasResponse
    {
        public GetDynamicTableMetasResponse(DynamicTableMeta[] response) => Response = response;
        public DynamicTableMeta[] Response { get; set; }
    }

    public class GetDynamicTableMetasRequest : IRequest<GetDynamicTableMetasResponse>
    {
        public GetDynamicTableMetasRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}