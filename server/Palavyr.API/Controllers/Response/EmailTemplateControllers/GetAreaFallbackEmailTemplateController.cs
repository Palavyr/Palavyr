using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class GetAreaFallbackEmailTemplateController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetAreaFallbackEmailTemplateController> logger;

        public GetAreaFallbackEmailTemplateController(IConfigurationRepository configurationRepository, ILogger<GetAreaFallbackEmailTemplateController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }
        
        /// <summary>
        /// This controller should only be used for getting the template for the editor. I.e. NO variable substitution.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("email/fallback/{areaId}/email-template")]
        public async Task<string> Get(string areaId, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            var emailTemplate = area.FallbackEmailTemplate;
            return emailTemplate;
        }
        
    }
}