using System.Linq;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.FormPaths;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class DeleteAreaController : ControllerBase
    {
        private ILogger<DeleteAreaController> logger;
        private DashContext dashContext;
        private IAmazonSimpleEmailService client { get; set; }
        
        public DeleteAreaController(
            IAmazonSimpleEmailService client,
            ILogger<DeleteAreaController> logger,
            DashContext dashContext)
        {
            this.logger = logger;
            this.client = client;
            this.dashContext = dashContext;
        }
        

        [HttpDelete("areas/delete/{areaId}")]
        public IActionResult Delete([FromHeader] string accountId, string areaId)
        {
            dashContext.Areas.RemoveRange(dashContext.Areas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.ConversationNodes.RemoveRange(dashContext.ConversationNodes.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.DynamicTableMetas.RemoveRange(dashContext.DynamicTableMetas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.FileNameMaps.RemoveRange(dashContext.FileNameMaps.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticFees.RemoveRange(dashContext.StaticFees.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticTablesMetas.RemoveRange(dashContext.StaticTablesMetas.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticTablesRows.RemoveRange(dashContext.StaticTablesRows.Where(row =>
                row.AreaIdentifier == areaId && row.AccountId == accountId));

            try
            {
                DiskUtils.DeleteAreaFolder(accountId, areaId);
                dashContext.SaveChanges();
            }
            catch
            {
                logger.LogCritical($"Area Data NOT Deleted.");
                logger.LogCritical($"Unable to delete the area folder for {accountId} under areaId {areaId}.");
            }

            return NoContent();
        }
    }
}