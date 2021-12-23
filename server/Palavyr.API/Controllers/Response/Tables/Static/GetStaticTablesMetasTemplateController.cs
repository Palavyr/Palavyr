using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTablesMetasTemplateController : PalavyrBaseController
    {
        private ILogger<GetStaticTablesMetasTemplateController> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public GetStaticTablesMetasTemplateController(ILogger<GetStaticTablesMetasTemplateController> logger, IHoldAnAccountId accountIdHolder)
        {
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }
        
        [HttpGet("response/configuration/{areaId}/static/tables/template")]
        public StaticTablesMeta CreateNewStaticTablesMeta(string areaId)
        {
            return StaticTablesMeta.CreateNewMetaTemplate(areaId, accountIdHolder.AccountId);
        }
    }
}