using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/SelectOneFlat")]
    [ApiController]
    public class SelectOneFlatController : DynamicControllerBase<SelectOneFlat>
    {
        public SelectOneFlatController(IDynamicTableCommandExecutor<SelectOneFlat> executor) : base(executor) { }
    }
}