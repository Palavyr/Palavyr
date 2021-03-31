using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response
{
    public class ModifyEpilogueConfigurationController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyEpilogueConfigurationController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("response/configuration/{areaId}/epilogue")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EpilogueReceiver epilogueReceiver
        )
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            area.Epilogue = epilogueReceiver.Epilogue;
            await configurationRepository.CommitChangesAsync();
            return epilogueReceiver.Epilogue;
        }
    }
}