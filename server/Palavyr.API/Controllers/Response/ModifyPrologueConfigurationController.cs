using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response
{
    public class ModifyPrologueConfigurationController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyPrologueConfigurationController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("response/configuration/{areaId}/prologue")]
        public async Task<string> UpdatePrologue(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] PrologueReceiver prologueReceiver)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            area.Prologue = prologueReceiver.Prologue;
            await configurationRepository.CommitChangesAsync();
            return prologueReceiver.Prologue;
        }
    }
}