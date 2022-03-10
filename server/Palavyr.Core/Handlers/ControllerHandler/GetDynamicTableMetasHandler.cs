using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDynamicTableMetasHandler : IRequestHandler<GetDynamicTableMetasRequest, GetDynamicTableMetasResponse>
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly ILogger<GetDynamicTableMetasHandler> logger;
        private readonly IUnitRetriever unitRetriever;

        public GetDynamicTableMetasHandler(
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            ILogger<GetDynamicTableMetasHandler> logger,
            IUnitRetriever unitRetriever)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.logger = logger;
            this.unitRetriever = unitRetriever;
        }

        public async Task<GetDynamicTableMetasResponse> Handle(GetDynamicTableMetasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await dynamicTableMetaStore.GetMany(request.IntentId, s => s.AreaIdentifier); 
            
            var tableResources = tableTypes.Select(
                    x =>
                    {
                        var unitDefinition = unitRetriever.GetUnitDefinitionById(x.UnitId);
                        return new DynamicTableMetaResource
                        {
                            Id = x.Id,
                            TableTag = x.TableTag,
                            TableType = x.TableType,
                            TableId = x.TableId,
                            AreaIdentifier = x.AreaIdentifier,
                            ValuesAsPaths = x.ValuesAsPaths,
                            PrettyName = x.PrettyName,
                            UnitPrettyName = unitDefinition.UnitPrettyName,
                            UnitGroup = unitDefinition.UnitGroup,
                            UnitId = unitDefinition.UnitId
                        };
                    })
                .ToArray();

            return new GetDynamicTableMetasResponse(tableResources);
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
        public string UnitPrettyName { get; set; }
        public string UnitGroup { get; set; }
        public UnitIds UnitId { get; set; }
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