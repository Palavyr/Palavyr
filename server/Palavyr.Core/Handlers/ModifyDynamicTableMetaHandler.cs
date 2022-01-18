using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Handlers
{
    public class ModifyDynamicTableMetaHandler : IRequestHandler<ModifyDynamicTableMetaRequest, ModifyDynamicTableMetaResponse>
    {
        private readonly DashContext dashContext;

        public ModifyDynamicTableMetaHandler(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }

        public async Task<ModifyDynamicTableMetaResponse> Handle(ModifyDynamicTableMetaRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new DomainException("Model Id is needed at this time");
            }

            var currentMeta = await dashContext.DynamicTableMetas.SingleAsync(x => x.Id == request.Id);

            currentMeta.AccountId = request.AccountId;
            currentMeta.TableTag = request.TableTag;
            currentMeta.TableType = request.TableType;
            currentMeta.TableId = request.TableId;
            currentMeta.AreaIdentifier = request.AreaId;
            currentMeta.ValuesAsPaths = request.ValueAsPaths;
            currentMeta.PrettyName = request.PrettyName;

            dashContext.DynamicTableMetas.Update(currentMeta);
            await dashContext.SaveChangesAsync(cancellationToken);
            return new ModifyDynamicTableMetaResponse(currentMeta);
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
        public string AccountId { get; set; }
        public string AreaId { get; set; }
        public bool ValueAsPaths { get; set; }
        public string PrettyName { get; set; }
    }
}