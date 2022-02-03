using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Handlers
{
    public class GetDynamicTableMetasHandler : IRequestHandler<GetDynamicTableMetasRequest, GetDynamicTableMetasResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<GetDynamicTableMetasHandler> logger;
        private readonly IUnitRetriever unitRetriever;

        public GetDynamicTableMetasHandler(IConfigurationRepository configurationRepository, ILogger<GetDynamicTableMetasHandler> logger, IUnitRetriever unitRetriever)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
            this.unitRetriever = unitRetriever;
        }

        public async Task<GetDynamicTableMetasResponse> Handle(GetDynamicTableMetasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await configurationRepository.GetDynamicTableMetas(request.IntentId);

            var tableResources = tableTypes.Select(
                x =>
                {
                    // var unitDefinition = unitRetriever.GetUnitDefinitionById(x.UnitId);
                    return new DynamicTableMetaResource
                    {
                        Id = x.Id,
                        TableTag = x.TableTag,
                        TableType = x.TableType,
                        TableId = x.TableId,
                        AreaIdentifier = x.AreaIdentifier,
                        ValuesAsPaths = x.ValuesAsPaths,
                        PrettyName = x.PrettyName,
                        // UnitId = unitDefinition.UnitId,
                        // UnitType = unitDefinition.UnitType
                    };
                });

            return new GetDynamicTableMetasResponse(tableResources.ToArray());
        }
    }

    public class DynamicTableMetaResource
    {
        public int? Id { get; set; }
        public string TableTag { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string AreaIdentifier { get; set; }
        public bool ValuesAsPaths { get; set; }
        public string PrettyName { get; set; }
        public string UnitId { get; set; }
        public string UnitType { get; set; }

        // public string AccountId { get; set; }
    }


    public class GetDynamicTableMetasResponse
    {
        public GetDynamicTableMetasResponse(DynamicTableMetaResource[] response) => Response = response;
        public DynamicTableMetaResource[] Response { get; set; }
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