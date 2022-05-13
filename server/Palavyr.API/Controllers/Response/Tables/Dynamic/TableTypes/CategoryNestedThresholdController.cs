using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/CategoryNestedThreshold")]
    [ApiController]
    public class CategoryNestedThresholdController : DynamicControllerBase<CategoryNestedThreshold, CategoryNestedThresholdResource>
    {
        public CategoryNestedThresholdController(
            IDynamicTableCommandExecutor<CategoryNestedThreshold> executor,
            IMapToNew<CategoryNestedThreshold, CategoryNestedThresholdResource> entityMapper,
            IMapToNew<DynamicTableData<CategoryNestedThreshold>, DynamicTableDataResource<CategoryNestedThresholdResource>> tableDataMapper) : base(executor, entityMapper, tableDataMapper)
        {
        }
    }
}