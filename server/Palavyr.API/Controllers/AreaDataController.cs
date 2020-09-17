using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.pathUtils;
using Palavyr.API.ReceiverTypes;
using Server.Domain;


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
            var areaData = DashContext.Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .Single(row => row.AreaIdentifier == areaId);
            
            DashContext.Areas.Remove(areaData);

            DashContext.ConversationNodes.RemoveRange(DashContext.ConversationNodes.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            DashContext.StaticTablesMetas.RemoveRange(DashContext.StaticTablesMetas.Where(row => row.AreaIdentifier == areaId&& row.AccountId == accountId));
            DashContext.StaticTablesRows.RemoveRange(DashContext.StaticTablesRows.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            
            DashContext.SaveChanges();

            DiskUtils.DeleteAreaFolder(accountId, areaId);
            
            return new OkResult();
        }
    }
}