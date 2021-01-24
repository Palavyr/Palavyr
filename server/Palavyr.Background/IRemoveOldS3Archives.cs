using System.Threading.Tasks;

namespace Palavyr.Background
{
    public interface IRemoveOldS3Archives
    {
        Task RemoveS3Objects();
    }
}