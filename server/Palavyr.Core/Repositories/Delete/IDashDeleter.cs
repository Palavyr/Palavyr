using System.Threading.Tasks;

namespace Palavyr.Core.Repositories.Delete
{
    public interface IDashDeleter : IConfigurationRepository
    {
        Task DeleteAccount();
        void DeleteAreasByAccount();
        void DeleteConvoNodesByAccount();
        void DeleteDynamicTableMetasByAccount();
        void DeleteFileNameMapsByAccount();
        void DeleteSelectOneFlatsByAccount();
        void DeletePercentOfThresholdByAccount();
        void DeleteStaticFeesByAccount();
        void DeleteStaticTableMetasByAccount();
        void DeleteStaticTableRowsByAccount();
        void DeleteWidgetPreferencesByAccount();
    }
}