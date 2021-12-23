using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class GetAreaEmailTemplateController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetAreaEmailTemplateController> logger;

        public GetAreaEmailTemplateController(IConfigurationRepository configurationRepository, ILogger<GetAreaEmailTemplateController> logger)
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
        [HttpGet("email/{areaId}/email-template")]
        public async Task<string> GetEmailTemplate(string areaId)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            var emailTemplate = area.EmailTemplate;
            return emailTemplate;
        }
        
    }
}