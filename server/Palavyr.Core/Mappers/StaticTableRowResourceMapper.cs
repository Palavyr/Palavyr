using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class StaticTableRowResourceMapper : IMapToNew<StaticTableRow, StaticTableRowResource>
    {
        private readonly IMapToNew<StaticFee, StaticFeeResource> feeMapper;

        public StaticTableRowResourceMapper(IMapToNew<StaticFee, StaticFeeResource> feeMapper)
        {
            this.feeMapper = feeMapper;
        }

        public async Task<StaticTableRowResource> Map(StaticTableRow @from)
        {
            await Task.CompletedTask;
            return new StaticTableRowResource
            {
                RowOrder = @from.RowOrder,
                Description = @from.Description,
                Fee = await feeMapper.Map(@from.Fee),
                Range = @from.Range,
                PerPerson = @from.PerPerson,
                TableOrder = @from.TableOrder,
                AreaIdentifier = @from.AreaIdentifier,
                AccountId = @from.AccountId
            };
        }
    }
}