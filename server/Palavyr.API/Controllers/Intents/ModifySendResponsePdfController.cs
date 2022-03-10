using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Intents
{
    public class ModifySendResponsePdfController : PalavyrBaseController
    {
        private readonly IEntityStore<Area> intentStore;

        private const string Route = "area/send-pdf/{intentId}";

        public ModifySendResponsePdfController(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        [HttpPost(Route)]
        public async Task<bool> Post(
            [FromRoute] string intentId,
            CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(intentId, s => s.AreaIdentifier);
            var newState = !area.SendPdfResponse;
            area.SendPdfResponse = newState;
            return newState;
        }
    }
}