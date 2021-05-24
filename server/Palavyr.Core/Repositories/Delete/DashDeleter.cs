using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Repositories.Delete
{
    public class DashDeleter : ConfigurationRepository, IDashDeleter
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private readonly IS3Deleter s3Deleter;
        private readonly ILogger<DashDeleter> logger;

        public DashDeleter(
            IConfiguration configuration,
            DashContext dashContext,
            IS3Deleter s3Deleter,
            ILogger<DashDeleter> logger
        ) : base(dashContext, logger)
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.s3Deleter = s3Deleter;
            this.logger = logger;
        }

        public void DeleteAccount(string accountId)
        {
            var userDataBucket = configuration.GetUserDataBucket();
            try
            {
                var s3Keys = dashContext.FileNameMaps.Where(x => x.AccountId == accountId).Select(x => x.S3Key).ToArray();
                s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3Keys);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var s3Keys = dashContext.Images.Where(x => x.AccountId == accountId).Select(x => x.S3Key).ToArray();
                s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3Keys);
            }
            catch (Exception)
            {
                // ignore
            }
            
            
            DeleteAreasByAccount(accountId);
            DeleteConvoNodesByAccount(accountId);
            DeleteDynamicTableMetasByAccount(accountId);
            DeleteFileNameMapsByAccount(accountId);
            DeleteStaticFeesByAccount(accountId);
            DeleteStaticTableMetasByAccount(accountId);
            DeleteStaticTableRowsByAccount(accountId);
            DeleteWidgetPreferencesByAccount(accountId);
            DeleteImagesByAccount(accountId);

            //dynamic tables
            DeleteSelectOneFlatsByAccount(accountId);
            DeletePercentOfThresholdByAccount(accountId);
            DeleteBasicThresholdByAccount(accountId);
            DeleteCategoryNestedThresholdByAccount(accountId);
            DeleteTwoNestedCategoriesByAccount(accountId);
        }

        public void DeleteImagesByAccount(string accountId)
        {
            logger.LogInformation($"Deleted Images for {accountId}");
            var images = dashContext.Images.Where(row => row.AccountId == accountId);
            dashContext.RemoveRange(images);
        }

        public void DeleteAreasByAccount(string accountId)
        {
            logger.LogInformation($"Deleted areas for {accountId}");
            var areas = dashContext.Areas.Where(row => row.AccountId == accountId);
            dashContext.Areas.RemoveRange(areas);
        }

        public void DeleteConvoNodesByAccount(string accountId)
        {
            logger.LogInformation($"Deleting convo nodes for {accountId}");
            var convoNodes = dashContext.ConversationNodes.Where(row => row.AccountId == accountId);
            dashContext.ConversationNodes.RemoveRange(convoNodes);
        }

        public void DeleteDynamicTableMetasByAccount(string accountId)
        {
            logger.LogInformation($"Deleting dynamic table metas for {accountId}");
            var dynamicTableMetas = dashContext.DynamicTableMetas.Where(row => row.AccountId == accountId);
            dashContext.DynamicTableMetas.RemoveRange(dynamicTableMetas);
        }

        public void DeleteFileNameMapsByAccount(string accountId)
        {
            logger.LogInformation($"Deleting file name maps for {accountId}");
            var nameMaps = dashContext.FileNameMaps.Where(row => row.AccountId == accountId);
            dashContext.FileNameMaps.RemoveRange(nameMaps);
        }

        public void DeleteSelectOneFlatsByAccount(string accountId)
        {
            logger.LogInformation($"Deleting selectOneFlat records for {accountId}");
            var selectOneFlats = dashContext.SelectOneFlats.Where(row => row.AccountId == accountId);
            dashContext.SelectOneFlats.RemoveRange(selectOneFlats);
        }

        public void DeletePercentOfThresholdByAccount(string accountId)
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountId}");
            var percentOfThreshold = dashContext.PercentOfThresholds.Where(row => row.AccountId == accountId);
            dashContext.PercentOfThresholds.RemoveRange(percentOfThreshold);
        }

        public void DeleteTwoNestedCategoriesByAccount(string accountId)
        {
            logger.LogInformation($"Deleting TwoNestedCategories for {accountId}");
            var twoNested = dashContext.TwoNestedCategories.Where(row => row.AccountId == accountId);
            dashContext.TwoNestedCategories.RemoveRange(twoNested);
        }

        public void DeleteCategoryNestedThresholdByAccount(string accountId)
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountId}");
            var categoryNested = dashContext.CategoryNestedThresholds.Where(row => row.AccountId == accountId);
            dashContext.CategoryNestedThresholds.RemoveRange(categoryNested);
        }

        public void DeleteBasicThresholdByAccount(string accountId)
        {
            logger.LogInformation($"Deleting percentOfThreshold for {accountId}");
            var basicThreshold = dashContext.BasicThresholds.Where(row => row.AccountId == accountId);
            dashContext.BasicThresholds.RemoveRange(basicThreshold);
        }

        public void DeleteStaticFeesByAccount(string accountId)
        {
            logger.LogInformation($"Removing static fees for {accountId}");
            var staticFees = dashContext.StaticFees.Where(row => row.AccountId == accountId);
            dashContext.StaticFees.RemoveRange(staticFees);
        }

        public void DeleteStaticTableMetasByAccount(string accountId)
        {
            logger.LogInformation($"Removing static table metas for {accountId}");
            var staticTableMetas = dashContext.StaticTablesMetas.Where(row => row.AccountId == accountId);
            dashContext.StaticTablesMetas.RemoveRange(staticTableMetas);
        }

        public void DeleteStaticTableRowsByAccount(string accountId)
        {
            logger.LogInformation($"Deleting static table rows for {accountId}");
            var staticTableRows = dashContext.StaticTablesRows.Where(row => row.AccountId == accountId);
            dashContext.StaticTablesRows.RemoveRange(staticTableRows);
        }

        public void DeleteWidgetPreferencesByAccount(string accountId)
        {
            logger.LogInformation($"Deleting widget preferences for {accountId}");
            var prefs = dashContext.WidgetPreferences.Where(row => row.AccountId == accountId);
            dashContext.WidgetPreferences.RemoveRange(prefs);
        }
    }
}