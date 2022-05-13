using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/BasicThreshold")]
    [ApiController]
    public class BasicThresholdController : DynamicControllerBase<BasicThreshold, BasicThresholdResource>
    {
        public BasicThresholdController(
            IDynamicTableCommandExecutor<BasicThreshold> executor,
            IMapToNew<BasicThreshold, BasicThresholdResource> entityMapper,
            IMapToNew<DynamicTableData<BasicThreshold>, DynamicTableDataResource<BasicThresholdResource>> tableDataMapper) : base(executor, entityMapper, tableDataMapper)
        {
        }
    }
}