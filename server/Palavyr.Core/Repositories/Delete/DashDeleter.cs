using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories.Delete
{
    public class DashDeleter : ConfigurationRepository, IDashDeleter
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private readonly IS3Deleter s3Deleter;
        private readonly ILogger<DashDeleter> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public DashDeleter(
            IConfiguration configuration,
            DashContext dashContext,
            IS3Deleter s3Deleter,
            ILogger<DashDeleter> logger,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport
        ) : base(dashContext, logger, accountIdTransport, cancellationTokenTransport)
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.s3Deleter = s3Deleter;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task DeleteAccount()
        {
            var userDataBucket = configuration.GetUserDataBucket();
            try
            {
                var s3Keys = dashContext.FileNameMaps.Where(x => x.AccountId == accountIdTransport.AccountId).Select(x => x.S3Key).ToArray();
                if (s3Keys.Length > 0)
                {
                    await s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3Keys);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var s3Keys = dashContext.Images.Where(x => x.AccountId == accountIdTransport.AccountId).Select(x => x.S3Key).ToArray();
                if (s3Keys.Length > 0)
                {
                    await s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3Keys);
                }
            }
            catch (Exception)
            {
                // ignore
            }


            DeleteAreasByAccount();
            DeleteConvoNodesByAccount();
            DeleteDynamicTableMetasByAccount();
            DeleteFileNameMapsByAccount();
            DeleteStaticFeesByAccount();
            DeleteStaticTableMetasByAccount();
            DeleteStaticTableRowsByAccount();
            DeleteWidgetPreferencesByAccount();
            DeleteImagesByAccount();

            //dynamic tables
            DeleteSelectOneFlatsByAccount();
            DeletePercentOfThresholdByAccount();
            DeleteBasicThresholdByAccount();
            DeleteCategoryNestedThresholdByAccount();
            DeleteTwoNestedCategoriesByAccount();
        }

        public void DeleteImagesByAccount()
        {
            logger.LogInformation($"Deleted Images for {accountIdTransport.AccountId}");
            var images = dashContext.Images.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.RemoveRange(images);
        }

        public void DeleteAreasByAccount()
        {
            logger.LogInformation($"Deleted areas for {accountIdTransport.AccountId}");
            var areas = dashContext.Areas.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.Areas.RemoveRange(areas);
        }

        public void DeleteConvoNodesByAccount()
        {
            logger.LogInformation($"Deleting convo nodes for {accountIdTransport.AccountId}");
            var convoNodes = dashContext.ConversationNodes.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.ConversationNodes.RemoveRange(convoNodes);
        }

        public void DeleteDynamicTableMetasByAccount()
        {
            logger.LogInformation($"Deleting dynamic table metas for {accountIdTransport.AccountId}");
            var dynamicTableMetas = dashContext.DynamicTableMetas.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.DynamicTableMetas.RemoveRange(dynamicTableMetas);
        }

        public void DeleteFileNameMapsByAccount()
        {
            logger.LogInformation($"Deleting file name maps for {accountIdTransport.AccountId}");
            var nameMaps = dashContext.FileNameMaps.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.FileNameMaps.RemoveRange(nameMaps);
        }

        public void DeleteSelectOneFlatsByAccount()
        {
            logger.LogInformation($"Deleting selectOneFlat records for {accountIdTransport.AccountId}");
            var selectOneFlats = dashContext.SelectOneFlats.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.SelectOneFlats.RemoveRange(selectOneFlats);
        }

        public void DeletePercentOfThresholdByAccount()
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountIdTransport.AccountId}");
            var percentOfThreshold = dashContext.PercentOfThresholds.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.PercentOfThresholds.RemoveRange(percentOfThreshold);
        }

        public void DeleteTwoNestedCategoriesByAccount()
        {
            logger.LogInformation($"Deleting TwoNestedCategories for {accountIdTransport.AccountId}");
            var twoNested = dashContext.TwoNestedCategories.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.TwoNestedCategories.RemoveRange(twoNested);
        }

        public void DeleteCategoryNestedThresholdByAccount()
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountIdTransport.AccountId}");
            var categoryNested = dashContext.CategoryNestedThresholds.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.CategoryNestedThresholds.RemoveRange(categoryNested);
        }

        public void DeleteBasicThresholdByAccount()
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountIdTransport.AccountId}");
            var basicThreshold = dashContext.BasicThresholds.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.BasicThresholds.RemoveRange(basicThreshold);
        }

        public void DeleteStaticFeesByAccount()
        {
            logger.LogInformation($"Removing static fees for {accountIdTransport.AccountId}");
            var staticFees = dashContext.StaticFees.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.StaticFees.RemoveRange(staticFees);
        }

        public void DeleteStaticTableMetasByAccount()
        {
            logger.LogInformation($"Removing static table metas for {accountIdTransport.AccountId}");
            var staticTableMetas = dashContext.StaticTablesMetas.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.StaticTablesMetas.RemoveRange(staticTableMetas);
        }

        public void DeleteStaticTableRowsByAccount()
        {
            logger.LogInformation($"Deleting static table rows for {accountIdTransport.AccountId}");
            var staticTableRows = dashContext.StaticTablesRows.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.StaticTablesRows.RemoveRange(staticTableRows);
        }

        public void DeleteWidgetPreferencesByAccount()
        {
            logger.LogInformation($"Deleting widget preferences for {accountIdTransport.AccountId}");
            var prefs = dashContext.WidgetPreferences.Where(row => row.AccountId == accountIdTransport.AccountId);
            dashContext.WidgetPreferences.RemoveRange(prefs);
        }
    }
}