using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/PercentOfThreshold")]
    [ApiController]
    public class PercentOfThresholdController : DynamicControllerBase<PercentOfThreshold, PercentOfThresholdResource>
    {
        public PercentOfThresholdController(
            IDynamicTableCommandExecutor<PercentOfThreshold> executor,
            IMapToNew<PercentOfThreshold, PercentOfThresholdResource> entityMapper,
            IMapToNew<DynamicTableData<PercentOfThreshold>, DynamicTableDataResource<PercentOfThresholdResource>> tableDataMapper) : base(executor, entityMapper, tableDataMapper)
        {
        }
    }
}