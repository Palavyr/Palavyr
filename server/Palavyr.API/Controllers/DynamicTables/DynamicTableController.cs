using System;
using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Configuration.constants;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    
    // [EnableCors(origins: "*", headers: "*", methods: "*")] 
    [Route("api/tables/dynamic/")]
    [ApiController]
    public class DynamicTableController : BaseController
    {
        public DynamicTableController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        // One controller for getting each table type. A separate call to get the type. Each table has a different
        // structure, and thus a different type. So we can't return multiple types from the same controller without
        // further generalization. This can be done later if its worth it. Adding a new controller for each type
        // isn't that big of a deal since we'll only have dozens of types probably. If we make money, then we can switch
        // to a generic pattern. Its just too complex to implement right now.
        
        
        /// <summary>
        /// originally used to pul a crazy string from the area table, but now should list off
        /// the current metas from the meta table for a given area
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("type/{areaId}")]
        public List<DynamicTableMeta> GetDynamicTableMetas([FromHeader] string accountId, string areaId)
        {
            var tableTypes = DashContext
                .DynamicTableMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            return tableTypes;
        }

        [HttpGet("availabletables")]
        public List<string> GetAvailableTables([FromHeader] string accountId)
        {
            var availableTables = DynamicTableTypes.GetAvailableTablePrettyNames();
            return availableTables;
        }

        [HttpPut("update")]
        public DynamicTableMeta UpdateDynamicTableMeta([FromHeader] string accountId, [FromBody] DynamicTableMeta dynamicTableMeta)
        {
            DashContext.DynamicTableMetas.Update(dynamicTableMeta);
            return dynamicTableMeta;
        }
        
        [HttpPost("{areaId}")]
        public DynamicTableMeta CreateNewDynamicTable([FromHeader] string accountId, string areaId)
        {
            var area = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);

            var dynamicTables = area.DynamicTableMetas.ToList();

            var tableId = Guid.NewGuid().ToString();

            var newTableMeta = DynamicTableMeta.CreateNew("default Tag", DynamicTableTypes.DefaultPrettyName, DynamicTableTypes.DefaultTable, tableId,
                areaId, accountId);
            
            dynamicTables.Add(newTableMeta);
            area.DynamicTableMetas = dynamicTables;
            var defaultTable = SelectOneFlat.CreateTemplate(accountId, areaId, tableId);
            DashContext.SelectOneFlats.Add(defaultTable);
            DashContext.SaveChanges();
            
            return newTableMeta;
        }
    }
}