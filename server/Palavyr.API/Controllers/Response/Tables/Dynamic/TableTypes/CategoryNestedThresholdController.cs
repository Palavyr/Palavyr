using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/CategoryNestedThreshold")]
    [ApiController]
    public class CategoryNestedThresholdController : DynamicControllerBase<CategoryNestedThreshold>
    {
        public CategoryNestedThresholdController(IDynamicTableCommandHandler<CategoryNestedThreshold> handler) : base(handler)
        {
        }
    }
}