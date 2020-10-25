using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.ReceiverTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{

    [Authorize]
    [Route("api/estimate/configuration")]
    [ApiController]
    public class ConfigurationController : BaseController
    {
        public ConfigurationController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        [Microsoft.AspNetCore.Mvc.HttpGet("{areaId}")]
        public Area GetEstimateConfiguration([FromHeader] string accountId, string areaId)
        {
            var areaData = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee);
            return areaData.Single(row => row.AreaIdentifier == areaId);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut("{areaId}/logue")]
        public StatusCodeResult UpdatePrologue([FromHeader] string accountId, string areaId, [Microsoft.AspNetCore.Mvc.FromBody] Logue logue)
        {
            var areaRow = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
            if (areaRow == null)
            {
                return new NotFoundResult();
            }

            if (logue.Epilogue != null)
            {
                areaRow.Epilogue = logue.Epilogue;
            }

            if (logue.Prologue != null)
            {
                areaRow.Prologue = logue.Prologue;
            }

            DashContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("{areaId}/static/tables/save")]
        public List<StaticTablesMeta> SaveStaticTablesMetas(
            string areaId,
            [FromHeader] string accountId,
            [FromBody] List<StaticTablesMeta> staticTableMetas)
        {
            var delMetas = DashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(x => x.StaticTableRows)
                .ThenInclude(x => x.Fee);

            foreach (var met in delMetas)
            {
                foreach (var row in met.StaticTableRows)
                {
                    DashContext.StaticFees.Remove(DashContext.StaticFees.Find(row.Fee.Id));
                    DashContext.StaticTablesRows.Remove(DashContext.StaticTablesRows.Find(row.Id));
                }

                DashContext.StaticTablesMetas.Remove(DashContext.StaticTablesMetas.Find(met.Id));
            }

            DashContext.SaveChanges();

            var clearedMetas = StaticTablesMeta.BindTemplateList(staticTableMetas, accountId);
            var currentArea = DashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            currentArea.StaticTablesMetas = clearedMetas;
            DashContext.SaveChanges();

            var tables = DashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            return tables;
        }

        [HttpGet("{areaId}/static/tables")]
        public List<StaticTablesMeta> GetStaticTablesMetas([FromHeader] string accountId, string areaId)
        {
            var tables = DashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(row => row.StaticTableRows)
                .ThenInclude(x => x.Fee);
            return tables.ToList();
        }

        /// <summary>
        /// Create a new staticTableMeta and add it to the database. Returns all static table metas for the given area
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("{areaId}/static/tables/template")]
        public StaticTablesMeta CreateNewStaticTablesMeta([FromHeader] string accountId, string areaId)
        {
            return StaticTablesMeta.CreateNewMetaTemplate(areaId, accountId);
        }
    }
}