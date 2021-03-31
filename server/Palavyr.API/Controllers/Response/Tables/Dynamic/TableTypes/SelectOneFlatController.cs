using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/SelectOneFlat")]
    [ApiController]
    public class SelectOneFlatController : DynamicControllerBase<SelectOneFlat>
    {
        public SelectOneFlatController(IDynamicTableCommandHandler<SelectOneFlat> handler) : base(handler) { }
    }
}