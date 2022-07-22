using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class StaticTablesMetaResourceMapper : IMapToNew<StaticTablesMeta, StaticTableMetaResource>
    {
        private readonly IMapToNew<StaticTableRow, StaticTableRowResource> staticTableRowMapper;

        public StaticTablesMetaResourceMapper(IMapToNew<StaticTableRow, StaticTableRowResource> staticTableRowMapper)
        {
            this.staticTableRowMapper = staticTableRowMapper;
        }

        public async Task<StaticTableMetaResource> Map(StaticTablesMeta @from, CancellationToken cancellationToken)
        {
            var tableRows = await staticTableRowMapper.MapMany(@from.StaticTableRows, cancellationToken);
            return new StaticTableMetaResource
            {
                TableOrder = @from.TableOrder,
                Description = @from.Description,
                IntentId = @from.IntentId,
                StaticTableRowResources = tableRows,
                PerPersonInputRequired = @from.PerPersonInputRequired,
                IncludeTotals = @from.IncludeTotals,
            };
        }
    }
}