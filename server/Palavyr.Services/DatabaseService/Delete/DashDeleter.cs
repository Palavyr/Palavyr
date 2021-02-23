using System.Linq;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.Services.DatabaseService.Delete
{
    public interface IDashDeleter : IDashConnector
    {
        void DeleteAccount(string accountId);
        void DeleteAreasByAccount(string accountId);
        void DeleteConvoNodesByAccount(string accountId);
        void DeleteDynamicTableMetasByAccount(string accountId);
        void DeleteFileNameMapsByAccount(string accountId);
        void DeleteSelectOneFlatsByAccount(string accountId);
        void DeletePercentOfThresholdByAccount(string accountId);
        void DeleteStaticFeesByAccount(string accountId);
        void DeleteStaticTableMetasByAccount(string accountId);
        void DeleteStaticTableRowsByAccount(string accountId);
        void DeleteWidgetPreferencesByAccount(string accountId);
    }

    public class DashDeleter : DashConnector, IDashDeleter
    {
        private readonly DashContext dashContext;
        private readonly ILogger<DashDeleter> logger;

        public DashDeleter(DashContext dashContext, ILogger<DashDeleter> logger)
        : base(dashContext, logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        public void DeleteAccount(string accountId)
        {
            DeleteAreasByAccount(accountId);
            DeleteConvoNodesByAccount(accountId);
            DeleteDynamicTableMetasByAccount(accountId);
            DeleteFileNameMapsByAccount(accountId);
            DeleteStaticFeesByAccount(accountId);
            DeleteStaticTableMetasByAccount(accountId);
            DeleteStaticTableRowsByAccount(accountId);
            DeleteWidgetPreferencesByAccount(accountId);
            
            //dynamic tables
            DeleteSelectOneFlatsByAccount(accountId);
            DeletePercentOfThresholdByAccount(accountId);
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
