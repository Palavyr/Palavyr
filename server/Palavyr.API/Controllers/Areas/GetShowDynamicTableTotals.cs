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
            string areaId,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            return area.IncludeDynamicTableTotals;
        }

        [HttpPut("area/dynamic-totals/{areaId}")]
        public async Task<bool> Post(
            [FromRoute]
            string areaId,
            [FromBody]
            ShouldShow request,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            area.IncludeDynamicTableTotals = request.ShowDynamicTotals;
            await configurationRepository.CommitChangesAsync();
            return area.IncludeDynamicTableTotals;
        }
    }

    public class ShouldShow
    {
        public bool ShowDynamicTotals { get; set; }
    }
}