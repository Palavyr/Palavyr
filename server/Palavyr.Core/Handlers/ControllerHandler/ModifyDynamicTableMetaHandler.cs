using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDynamicTableMetaHandler : IRequestHandler<ModifyDynamicTableMetaRequest, ModifyDynamicTableMetaResponse>
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly IUnitRetriever unitRetriever;

        public ModifyDynamicTableMetaHandler(
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            IUnitRetriever unitRetriever)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.unitRetriever = unitRetriever;
        }

        public async Task<ModifyDynamicTableMetaResponse> Handle(ModifyDynamicTableMetaRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new DomainException("Model Id is needed at this time");
            }

            var currentMeta = await dynamicTableMetaStore.Get(request.TableId, s => s.TableId);
            currentMeta.UpdateProperties(request, unitRetriever);

            var updatedMeta = await dynamicTableMetaStore.Update(currentMeta);

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