﻿using System.Threading.Tasks;

namespace Palavyr.Core.Repositories.Delete
{
    public interface IDashDeleter : IConfigurationRepository
    {
        Task DeleteAccount(string accountId);
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
}