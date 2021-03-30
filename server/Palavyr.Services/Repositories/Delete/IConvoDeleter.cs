namespace Palavyr.Services.Repositories.Delete
{
    public interface IConvoDeleter : IConvoHistoryRepository
    {
        void DeleteAccount(string accountId);
        void DeleteAllConversationRecordsByAccount(string accountId);
        void DeleteAllCompletedConversationsByAccount(string accountId);
    }
}