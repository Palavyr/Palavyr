using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/SelectOneFlat")]
    [ApiController]
    public class SelectOneFlatController : DynamicControllerBase<SelectOneFlat>
    {
        public SelectOneFlatController(IDynamicTableCommandHandler<SelectOneFlat> handler) : base(handler) { }
    }
}