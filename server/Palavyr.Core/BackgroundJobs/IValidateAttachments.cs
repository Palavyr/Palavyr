namespace Palavyr.Core.BackgroundJobs
{
    public interface IValidateAttachments
    {
        void ValidateAllAttachments();
        void ValidateAllFiles();
    }
}