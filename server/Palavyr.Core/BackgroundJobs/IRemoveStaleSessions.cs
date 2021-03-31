namespace Palavyr.Core.BackgroundJobs
{
    public interface IRemoveStaleSessions
    {
        void CleanSessionDB();
    }
}