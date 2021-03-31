using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/TwoNestedCategory")]
    [ApiController]
    public class TwoNestedCategoriesController : DynamicControllerBase<PercentOfThreshold>
    {
        public TwoNestedCategoriesController(IDynamicTableCommandHandler<PercentOfThreshold> handler) : base(handler)
        {
        }
    }
}