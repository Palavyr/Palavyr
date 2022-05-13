using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/SelectOneFlat")]
    public class SelectOneFlatController : DynamicControllerBase<SelectOneFlat, SelectOneFlatRowResource>
    {
        public SelectOneFlatController(
            IDynamicTableCommandExecutor<SelectOneFlat> executor,
            IMapToNew<SelectOneFlat, SelectOneFlatRowResource> entityMapper,
            IMapToNew<DynamicTableData<SelectOneFlat>, DynamicTableDataResource<SelectOneFlatRowResource>> tableDataMapper) : base(executor, entityMapper, tableDataMapper)
        {
        }
    }
}