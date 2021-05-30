using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;

namespace Palavyr.Core.Services.EmailService.Verification
{
    public interface IEmailVerificationStatus
    {
        Task<(bool, IdentityVerificationAttributes)> RequestEmailVerificationStatus(string emailAddress);
        Task<bool> CheckVerificationStatus(string emailAddress);
        EmailVerificationResponse HandleFoundEmail(IdentityVerificationAttributes status, string emailAddress);
        Task<EmailVerificationResponse> HandleNotFoundEmail(IdentityVerificationAttributes status, string emailAddress);
        Task<EmailVerificationResponse> GetVerificationResponse(string emailAddress);
    }
}