using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPasswordHandler : IRequestHandler<ModifyPasswordRequest, ModifyPasswordResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ModifyPasswordHandler(
            IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ModifyPasswordResponse> Handle(ModifyPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            if (!PasswordHashing.ComparePasswords(account.Password, request.OldPassword))
            {
                throw new DomainException("Original password does not match that on record");
            }

            var hashedNewPassword = PasswordHashing.CreateHashedPassword(request.Password);
            account.Password = hashedNewPassword;
            
            return new ModifyPasswordResponse(true);
        }
    }

    public class ModifyPasswordResponse
    {
        public ModifyPasswordResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyPasswordRequest : IRequest<ModifyPasswordResponse>
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}