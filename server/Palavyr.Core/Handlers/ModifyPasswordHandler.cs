using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyPasswordHandler : IRequestHandler<ModifyPasswordRequest, ModifyPasswordResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ModifyPasswordHandler(
            IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyPasswordResponse> Handle(ModifyPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            var oldHashedPassword = request.OldPassword;
            if (oldHashedPassword != request.Password)
            {
                return new ModifyPasswordResponse(false);
            }

            account.Password = request.Password;
            await accountRepository.CommitChangesAsync();
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