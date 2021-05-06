using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTablesMetasTemplateController : PalavyrBaseController
    {
        private ILogger<GetStaticTablesMetasTemplateController> logger;

        public GetStaticTablesMetasTemplateController(ILogger<GetStaticTablesMetasTemplateController> logger)
        {
            this.logger = logger;
        }
        
        [HttpGet("response/configuration/{areaId}/static/tables/template")]
        public StaticTablesMeta CreateNewStaticTablesMeta([FromHeader] string accountId, string areaId)
        {
            return StaticTablesMeta.CreateNewMetaTemplate(areaId, accountId);
        }
    }
}