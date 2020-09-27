using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Palavyr.API.receiverTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/tables/dynamic/SelectOneFlat")]
    [ApiController]
    public class SelectOneFlatController : BaseController, IDynamicTableController
    {
        public SelectOneFlatController(AccountsContext accountContext, ConvoContext convoContext,
            DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        [HttpGet("tableId/{tableId}/data/{areaId}/")]
        public List<SelectOneFlat> GetDynamicTableRows([FromHeader] string accountId, string areaId, string tableId)
        {
            var oneFlats = DashContext.SelectOneFlats.Where(
                row => row.AccountId == accountId
                       && row.AreaIdentifier == areaId
                       && row.TableId == tableId).ToList();
            return oneFlats;
        }

        [HttpGet("data/template/{areaId}/{tableId}")]
        public SelectOneFlat GetDynamicRowTemplate([FromHeader] string accountId, string areaId, string tableId)
        {
            var template = SelectOneFlat.CreateTemplate(accountId, areaId, tableId);
            return template;
        }

        [HttpPut("data/save/tableId/{tableId}/{areaId}")]
        public List<SelectOneFlat> SaveDynamicTable(
            string areaId, 
            string tableId,
            [FromHeader] string accountId, 
            [FromBody] DynamicTable dynamicTable)
        {
            DashContext.SelectOneFlats.RemoveRange(DashContext
                .SelectOneFlats
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            var mappedTableRows = new List<SelectOneFlat>();
            foreach (var row in dynamicTable.SelectOneFlat)
            {
                var mappedRow = SelectOneFlat.CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.Option,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.TableId,
                    row.TableTag
                );
                mappedTableRows.Add(mappedRow);
            }

            DashContext.SelectOneFlats.AddRange(mappedTableRows);
            
            var meta = DashContext.DynamicTableMetas.Single(row => row.TableId == tableId);
            meta.TableTag = dynamicTable.TableTag;

            DashContext.SaveChanges();

            return DashContext.SelectOneFlats
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId)
                .ToList();
        }

        [HttpDelete("{areaId}/tableId/{tableId}")]
        public StatusCodeResult DeleteDynamicTable([FromHeader] string accountId, string areaId, string tableId)
        {
            DashContext
                .DynamicTableMetas
                .Remove(
                    DashContext
                        .DynamicTableMetas
                        .Single(row =>
                            row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            DashContext
                .SelectOneFlats
                .RemoveRange(
                    DashContext.SelectOneFlats.Where(row =>
                        row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            DashContext.SaveChanges();

            return new OkResult();
        }
    }
}