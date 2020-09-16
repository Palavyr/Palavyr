using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace Palavyr.Background
{
    public interface IRemoveOldS3Archives
    {
        Task RemoveS3Objects();
    }
}