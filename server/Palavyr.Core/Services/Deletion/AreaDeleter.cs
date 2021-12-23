using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.Deletion
{
    public interface IAreaDeleter
    {
        Task DeleteArea(string areaId, CancellationToken cancellationToken);
    }

    public class AreaDeleter : IAreaDeleter
    {
        private readonly DashContext dashContext;
        private readonly IS3Deleter s3Deleter;
        private readonly IConfiguration configuration;
        private readonly ILogger<IAreaDeleter> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public AreaDeleter(
            DashContext dashContext,
            IS3Deleter s3Deleter,
            IConfiguration configuration,
            ILogger<IAreaDeleter> logger, 
            IHoldAnAccountId accountIdHolder
        )
        {
            this.dashContext = dashContext;
            this.s3Deleter = s3Deleter;
            this.configuration = configuration;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task DeleteArea(string areaId, CancellationToken cancellationToken)
        {
            await DeleteS3Data(accountIdHolder.AccountId, areaId, cancellationToken);
            DeleteDatabaseEntries( accountIdHolder.AccountId, areaId);

            try
            {
                await dashContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                logger.LogCritical($"Area Data NOT Deleted.");
                logger.LogCritical($"Unable to delete the area folder for {accountIdHolder.AccountId} under areaId {areaId}.");
            }
        }

        private async Task DeleteS3Data(string accountId, string areaId, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetUserDataBucket();
            var s3KeysToDelete = await dashContext.FileNameMaps
                .Where(x => x.AreaIdentifier == areaId && x.AccountId == accountId)
                .Select(x => x.S3Key)
                .ToArrayAsync(cancellationToken);
            await s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3KeysToDelete);
        }

        private void DeleteDatabaseEntries(string accountId, string areaId)
        {
            DeleteConfiguration(accountId, areaId);
            DeleteDynamicTableData(accountId, areaId);
        }

        private void DeleteConfiguration(string accountId, string areaId)
        {
            dashContext.Areas.RemoveRange(
                dashContext.Areas.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.ConversationNodes.RemoveRange(
                dashContext.ConversationNodes.Where(
                    row =>
                        row.AreaIdentifier == areaId && row.AccountId == accountId));

            dashContext.FileNameMaps.RemoveRange(
                dashContext.FileNameMaps.Where(
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
        }

        private void DeleteDynamicTableData(string accountId, string areaId)
        {
            dashContext.DynamicTableMetas.RemoveRange(dashContext.DynamicTableMetas.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.PercentOfThresholds.RemoveRange(dashContext.PercentOfThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.BasicThresholds.RemoveRange(dashContext.BasicThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.TwoNestedCategories.RemoveRange(dashContext.TwoNestedCategories.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
            dashContext.CategoryNestedThresholds.RemoveRange(dashContext.CategoryNestedThresholds.Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId));
        }
    }
}