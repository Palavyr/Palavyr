namespace Palavyr.Core.Repositories.Delete
{
    public interface IConvoDeleter : IConvoHistoryRepository
    {
        void DeleteAccount();
        void DeleteAllConversationRecordsByAccount();
        void DeleteAllCompletedConversationsByAccount();
    }
}