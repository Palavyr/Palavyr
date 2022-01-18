using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/CategoryNestedThreshold")]
    [ApiController]
    public class CategoryNestedThresholdController : DynamicControllerBase<CategoryNestedThreshold>
    {
        public CategoryNestedThresholdController(IDynamicTableCommandExecutor<CategoryNestedThreshold> executor) : base(executor)
        {
        }
    }
}