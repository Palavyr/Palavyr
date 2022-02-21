using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ResetMyPasswordHandler : IRequestHandler<ResetMyPasswordRequest, ResetMyPasswordResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ResetMyPasswordHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ResetMyPasswordResponse> Handle(ResetMyPasswordRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResetMyPasswordResponse(new ResetPasswordResponse("Email address and password must both be set.", false));
            }

            var account = await accountRepository.GetAccount();

            // TODO: Consider password validation?
            account.Password = PasswordHashing.CreateHashedPassword(request.Password);
            await accountRepository.CommitChangesAsync();

            return new ResetMyPasswordResponse(new ResetPasswordResponse("Successfully reset your password. Return to the homepage to login with your new password.", true));
        }
    }

    public class ResetPasswordResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }

        public ResetPasswordResponse(string message, bool status)
        {
            Message = message;
            Status = status;
        }
    }

    public class ResetMyPasswordResponse
    {
        public ResetMyPasswordResponse(ResetPasswordResponse response) => Response = response;
        public ResetPasswordResponse Response { get; set; }
    }

    public class ResetMyPasswordRequest : IRequest<ResetMyPasswordResponse>
    {
        public string Password { get; set; }
    }
}