using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPhoneNumberHandler : IRequestHandler<ModifyPhoneNumberRequest, ModifyPhoneNumberResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ModifyPhoneNumberHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyPhoneNumberResponse> Handle(ModifyPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            account.PhoneNumber = request.PhoneNumber ?? "";
            await accountRepository.CommitChangesAsync();
            return new ModifyPhoneNumberResponse(account.PhoneNumber);
        }
    }

    public class ModifyPhoneNumberResponse
    {
        public ModifyPhoneNumberResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyPhoneNumberRequest : IRequest<ModifyPhoneNumberResponse>
    {
        public string PhoneNumber { get; set; }
    }
}