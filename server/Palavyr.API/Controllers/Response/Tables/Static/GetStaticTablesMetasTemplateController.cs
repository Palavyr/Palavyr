using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    [Authorize]
    public class GetStaticTablesMetasTemplateController : PalavyrBaseController
    {
        private ILogger<GetStaticTablesMetasTemplateController> logger;

        public GetStaticTablesMetasTemplateController(ILogger<GetStaticTablesMetasTemplateController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Create a new staticTableMeta and add it to the database. Returns all static table metas for the given area
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("response/configuration/{areaId}/static/tables/template")]
        public StaticTablesMeta CreateNewStaticTablesMeta([FromHeader] string accountId, string areaId)
        {
            return StaticTablesMeta.CreateNewMetaTemplate(areaId, accountId);
        }
    }
}