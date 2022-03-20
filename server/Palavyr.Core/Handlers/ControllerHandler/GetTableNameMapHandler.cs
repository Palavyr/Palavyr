using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetTableNameMapHandler : IRequestHandler<GetTableNameMapRequest, GetTableNameMapResponse>
    {
        public async Task<GetTableNameMapResponse> Handle(GetTableNameMapRequest request, CancellationToken cancellationToken)
        {
            // map that provides e.g. Select One Flat: SelectOneFlat.
            await Task.CompletedTask;
            var availableTables = DynamicTableTypes.GetDynamicTableTypes();
            var tableNameMap = new Dictionary<string, string>();
            foreach (var table in availableTables)
            {
                tableNameMap.Add(table.PrettyName, table.TableType);
            }

            return new GetTableNameMapResponse(tableNameMap);
        }
    }

    public class GetTableNameMapResponse
    {
        public GetTableNameMapResponse(Dictionary<string, string> response) => Response = response;
        public Dictionary<string, string> Response { get; set; }
    }

    public class GetTableNameMapRequest : IRequest<GetTableNameMapResponse>
    {
    }
}