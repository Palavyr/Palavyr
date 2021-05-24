using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    public class ModifySendResponsePdfController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        private const string Route = "area/send-pdf/{areaId}";

        public ModifySendResponsePdfController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPost(Route)]
        public async Task<bool> Post(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            var newState = !area.SendPdfResponse;
            area.SendPdfResponse = newState;
            await configurationRepository.CommitChangesAsync(cancellationToken);
            return newState;
        }
    }
}