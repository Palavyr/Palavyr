using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetShowSeenEnquiriesController : PalavyrBaseController
    {
        private readonly IAccountRepository repository;
        public const string Route = "enquiries/show";
        public GetShowSeenEnquiriesController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet(Route)]
        public async Task<bool> Get()
        {
            var account = await repository.GetAccount();
            var show = account.ShowSeenEnquiries;
            return show;
        }
    }
}