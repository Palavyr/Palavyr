using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Enquiries
{
    public class ToggleShowSeenEnquiriesController : PalavyrBaseController
    {
        private readonly IAccountRepository repository;
        public const string Route = "enquiries/toggle-show";

        public ToggleShowSeenEnquiriesController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        [HttpPut(Route)]
        public async Task<bool> Put()
        {
            var account = await repository.GetAccount();

            var newValue = !account.ShowSeenEnquiries;
            account.ShowSeenEnquiries = newValue;
            await repository.CommitChangesAsync();
            return newValue;
        }
    }
}