using System.Threading.Tasks;

namespace Palavyr.Core.Validators
{
    public interface INotificationValidator<T>
    {
        Task Validate(T notification);
    }
}