using System.Threading.Tasks;

namespace Palavyr.Core.BackgroundJobs
{
    public interface IRemoveOldS3Archives
    {
        Task RemoveS3Objects();
    }
}