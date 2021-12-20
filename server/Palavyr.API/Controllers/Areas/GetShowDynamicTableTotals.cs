using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    public class GetSingleAreaShallow : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetSingleAreaShallow(ILogger<GetSingleAreaShallow> logger, IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpGet("area/dynamic-totals/{areaId}")]
        public async Task<bool> Get(
            [FromHeader]
            string accountId,
            string areaId,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            return area.IncludeDynamicTableTotals;
        }

        [HttpPut("area/dynamic-totals/{areaId}")]
        public async Task<bool> Post(
            [FromHeader]
            string accountId,
            [FromRoute]
            string areaId,
            [FromBody]
            ShouldShow request,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            area.IncludeDynamicTableTotals = request.ShowDynamicTotals;
            await configurationRepository.CommitChangesAsync(cancellationToken);
            return area.IncludeDynamicTableTotals;
        }
    }

    public class ShouldShow
    {
        public bool ShowDynamicTotals { get; set; }
    }
}