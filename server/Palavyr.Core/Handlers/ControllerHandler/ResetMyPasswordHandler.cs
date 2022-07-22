using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ResetMyPasswordHandler : IRequestHandler<ResetMyPasswordRequest, ResetMyPasswordResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ResetMyPasswordHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ResetMyPasswordResponse> Handle(ResetMyPasswordRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResetMyPasswordResponse(new ResetPasswordResource("Email address and password must both be set.", false));
            }

            var account = await accountStore.GetAccount();

            // TODO: Consider password validation?
            account.Password = PasswordHashing.CreateHashedPassword(request.Password);
            
            return new ResetMyPasswordResponse(new ResetPasswordResource("Successfully reset your password. Return to the homepage to login with your new password.", true));
        }
    }

    public class ResetPasswordResource
    {
        public string Message { get; set; }
        public bool Status { get; set; }

        public ResetPasswordResource(string message, bool status)
        {
            Message = message;
            Status = status;
        }
    }

    public class ResetMyPasswordResponse
    {
        public ResetMyPasswordResponse(ResetPasswordResource resource) => Resource = resource;
        public ResetPasswordResource Resource { get; set; }
    }

    public class ResetMyPasswordRequest : IRequest<ResetMyPasswordResponse>
    {
        public string Password { get; set; }
    }
}