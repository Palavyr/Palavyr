using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class GetStaticTablesMetasController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private ILogger<GetStaticTablesMetasController> logger;

        public GetStaticTablesMetasController(IDashConnector dashConnector, ILogger<GetStaticTablesMetasController> logger)
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }
        
        [HttpGet("response/configuration/{areaId}/static/tables")]
        public async Task<List<StaticTablesMeta>> GetStaticTablesMetas([FromHeader] string accountId, string areaId)
        {
            var staticTables = await dashConnector.GetStaticTables(accountId, areaId);
            return staticTables;
        }
    }
}