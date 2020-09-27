using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.pathUtils;
using Palavyr.API.ReceiverTypes;
using Server.Domain;
using Server.Domain.Configuration.schema;


namespace Palavyr.API.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreaDataController : BaseController
    {
        public AreaDataController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        // GET: api/Areas
        [HttpGet]
        public IQueryable<Area> GetAllAreas([FromHeader] string accountId)
        {
            var area = DashContext.Set<Area>().Where(row => row.AccountId == accountId);
            return area;
        }
        
        [HttpGet("{areaId}")]
        public Area GetAreaById([FromHeader] string accountId, string areaId)
        {            
            var data = DashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            return data;
        }
        
        [HttpPost("create")]
        public Area CreateArea([FromHeader] string accountId, [FromBody] Text text)
        {
            var defaultAreaTemplate = Area.CreateNewArea(text.AreaName, accountId);
            var result = DashContext.Areas.Add(defaultAreaTemplate);
            DashContext.SaveChanges();
            return result.Entity;
        }

        [HttpPut("update/{areaId}")]
        public StatusCodeResult UpdateArea([FromHeader] string accountId, [FromBody] Text text, string areaId)
        {
            var newAreaName = text.AreaName;
            var newAreaDisplayTitle = text.AreaDisplayTitle;

            var curArea = DashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            
            if (text.AreaName != null)
                curArea.AreaName = newAreaName;
            if (text.AreaDisplayTitle != null)
                curArea.AreaDisplayTitle = newAreaDisplayTitle;
            DashContext.SaveChanges();
            return new OkResult();
        }
        
        [HttpDelete("delete/{areaId}")]
        public StatusCodeResult DeleteArea([FromHeader] string accountId, string areaId)
        {
            DashContext.Areas.RemoveRange( DashContext.Areas.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.ConversationNodes.RemoveRange(DashContext.ConversationNodes.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.DynamicTableMetas.RemoveRange(DashContext.DynamicTableMetas.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.FileNameMaps.RemoveRange(DashContext.FileNameMaps.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.SelectOneFlats.RemoveRange(DashContext.SelectOneFlats.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticFees.RemoveRange(DashContext.StaticFees.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticTablesMetas.RemoveRange(DashContext.StaticTablesMetas.Where(row => row.AreaIdentifier == areaId&& row.AccountId == accountId));
            DashContext.StaticTablesRows.RemoveRange(DashContext.StaticTablesRows.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.SaveChanges();

            DiskUtils.DeleteAreaFolder(accountId, areaId);
            
            return new OkResult();
        }
    }
}