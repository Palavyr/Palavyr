using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.Core.Mappers
{
    public class EmailVerificationResourceMapper : IMapToNew<EmailVerificationResponse, EmailVerificationResource>
    {
        public async Task<EmailVerificationResource> Map(EmailVerificationResponse from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return new EmailVerificationResource
            {
                Title = from.Title,
                Message = from.Message,
                Status = from.Status
            };
        }
    }
}