using System.Linq;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    public class DeleteAreaController : PalavyrBaseController
    {
        private ILogger<DeleteAreaController> logger;
        private DashContext dashContext;
        private readonly IGenericDynamicTableRepository<SelectOneFlat> selectOneFlatRepository;
        private readonly IGenericDynamicTableRepository<TwoNestedCategory> twonestedRepository;
        private readonly IGenericDynamicTableRepository<BasicThreshold> basicThresholdRepository;
        private readonly IGenericDynamicTableRepository<PercentOfThreshold> percentRespository;
        private IAmazonSimpleEmailService client { get; set; }

        public DeleteAreaController(
            IAmazonSimpleEmailService client,
            ILogger<DeleteAreaController> logger,
            DashContext dashContext,
            IGenericDynamicTableRepository<SelectOneFlat> selectOneFlatRepository,
            IGenericDynamicTableRepository<TwoNestedCategory> twonestedRepository,
            IGenericDynamicTableRepository<BasicThreshold> basicThresholdRepository,
            IGenericDynamicTableRepository<PercentOfThreshold> percentRespository
        )
        {
            this.logger = logger;
            this.client = client;
            this.dashContext = dashContext;
            this.selectOneFlatRepository = selectOneFlatRepository;
            this.twonestedRepository = twonestedRepository;
            this.basicThresholdRepository = basicThresholdRepository;
            this.percentRespository = percentRespository;
        }


        [HttpDelete("areas/delete/{areaId}")]
        public IActionResult Delete([FromHeader] string accountId, string areaId)
        {
            dashContext.Areas.RemoveRange(
                dashContext.Areas.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.ConversationNodes.RemoveRange(
                dashContext.ConversationNodes.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.DynamicTableMetas.RemoveRange(
                dashContext.DynamicTableMetas.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.FileNameMaps.RemoveRange(
                dashContext.FileNameMaps.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.SelectOneFlats.RemoveRange(
                dashContext.SelectOneFlats.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticFees.RemoveRange(
                dashContext.StaticFees.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticTablesMetas.RemoveRange(
                dashContext.StaticTablesMetas.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.StaticTablesRows.RemoveRange(
                dashContext.StaticTablesRows.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.BasicThresholds.RemoveRange((dashContext.BasicThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId)));

            dashContext.DynamicTableMetas.RemoveRange(dashContext.DynamicTableMetas.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.PercentOfThresholds.RemoveRange(dashContext.PercentOfThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.BasicThresholds.RemoveRange(dashContext.BasicThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.TwoNestedCategories.RemoveRange(dashContext.TwoNestedCategories.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            
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