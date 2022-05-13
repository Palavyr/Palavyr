using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/TwoNestedCategory")]
    [ApiController]
    public class TwoNestedCategoriesController : DynamicControllerBase<TwoNestedCategory, TwoNestedCategoryResource>
    {
        public TwoNestedCategoriesController(
            IDynamicTableCommandExecutor<TwoNestedCategory> executor,
            IMapToNew<TwoNestedCategory, TwoNestedCategoryResource> entityMapper,
            IMapToNew<DynamicTableData<TwoNestedCategory>, DynamicTableDataResource<TwoNestedCategoryResource>> tableDataMapper)
            : base(executor, entityMapper, tableDataMapper)
        {
        }
    }
}