using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/TwoNestedCategory")]
    [ApiController]
    public class TwoNestedCategoriesController : DynamicControllerBase<TwoNestedCategory>
    {
        public TwoNestedCategoriesController(IDynamicTableCommandExecutor<TwoNestedCategory> executor) : base(executor)
        {
        }
    }
}