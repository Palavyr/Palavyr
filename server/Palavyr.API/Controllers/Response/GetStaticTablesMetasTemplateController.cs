using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr1.API.Controllers.Response
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetStaticTablesMetasTemplateController : ControllerBase
    {
        private ILogger<GetStaticTablesMetasTemplateController> logger;

        public GetStaticTablesMetasTemplateController(ILogger<GetStaticTablesMetasTemplateController> logger)
        {
            this.logger = logger;
        }
        
        /// <summary>
        /// Create a new staticTableMeta and add it to the database. Returns all static table metas for the given area
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("response/configuration/{areaId}/static/tables/template")]
        public StaticTablesMeta CreateNewStaticTablesMeta([FromHeader] string accountId, string areaId)
        {
            return StaticTablesMeta.CreateNewMetaTemplate(areaId, accountId);
        }
    }
}