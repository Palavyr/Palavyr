using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDynamicTableMetaHandler : IRequestHandler<ModifyDynamicTableMetaRequest, ModifyDynamicTableMetaResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly DashContext dashContext;
        private readonly IUnitRetriever unitRetriever;

        public ModifyDynamicTableMetaHandler(
            IConfigurationRepository configurationRepository,
            DashContext dashContext,
            IUnitRetriever unitRetriever)
        {
            this.configurationRepository = configurationRepository;
            this.dashContext = dashContext;
            this.unitRetriever = unitRetriever;
        }

        public async Task<ModifyDynamicTableMetaResponse> Handle(ModifyDynamicTableMetaRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new DomainException("Model Id is needed at this time");
            }

            var currentMeta = await configurationRepository.GetDynamicTableMetaByTableId(request.TableId);
            currentMeta.UpdateProperties(request, unitRetriever);
            var updatedMeta = await configurationRepository.UpdateDynamicTableMeta(currentMeta);

            await configurationRepository.CommitChangesAsync();
            
            return new ModifyDynamicTableMetaResponse(updatedMeta);
        }
    }

    public class ModifyDynamicTableMetaResponse
    {
        public ModifyDynamicTableMetaResponse(DynamicTableMeta response) => Response = response;
        public DynamicTableMeta Response { get; set; }
    }

    public class ModifyDynamicTableMetaRequest : IRequest<ModifyDynamicTableMetaResponse>
    {
        public int Id { get; set; }
        public string TableTag { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string AreaIdentifier { get; set; }
        public bool ValueAsPaths { get; set; }
        public string PrettyName { get; set; }
        public string UnitGroup { get; set; }
        public string UnitPrettyName { get; set; }
        public int UnitId { get; set; }
    }
}