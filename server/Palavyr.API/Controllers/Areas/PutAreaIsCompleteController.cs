using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class PutAreaIsCompleteController : PalavyrBaseController
    {
        private readonly IDashConnector dashConnector;

        public PutAreaIsCompleteController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }

        [HttpPut("areas/{areaId}/area-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutAreaIsCompleteRequest request)
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.IsEnabled = request.IsEnabled;
            await dashConnector.CommitChangesAsync();
            return area.IsEnabled;
        }

        public class PutAreaIsCompleteRequest
        {
            public bool IsEnabled { get; set; }
        }
    }
}