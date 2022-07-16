using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class StaticTablesMetaResourceMapper : IMapToNew<StaticTablesMeta, StaticTablesMetaResource>
    {
        private readonly IMapToNew<StaticTableRow, StaticTableRowResource> staticTableRowMapper;

        public StaticTablesMetaResourceMapper(IMapToNew<StaticTableRow, StaticTableRowResource> staticTableRowMapper)
        {
            this.staticTableRowMapper = staticTableRowMapper;
        }
        public async Task<StaticTablesMetaResource> Map(StaticTablesMeta @from, CancellationToken cancellationToken)
        {
            var tableRows = await staticTableRowMapper.MapMany(@from.StaticTableRows, cancellationToken);
            return new StaticTablesMetaResource
            {
                TableOrder = @from.TableOrder,
                Description = @from.Description,
                AreaIdentifier = @from.IntentId,
                StaticTableRows = tableRows,
                AccountId = @from.AccountId,
                PerPersonInputRequired = @from.PerPersonInputRequired,
                IncludeTotals = @from.IncludeTotals
            };
        }
    }
}