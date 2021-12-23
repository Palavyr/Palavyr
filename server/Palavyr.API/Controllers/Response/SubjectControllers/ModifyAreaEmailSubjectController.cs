using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    
    [Authorize]

    public class ModifyAreaEmailSubjectController : PalavyrBaseController
    {
        private readonly IConfigurationRepository repository;
        private readonly ILogger<ModifyAreaEmailSubjectController> logger;

        public ModifyAreaEmailSubjectController(
            IConfigurationRepository repository,
            ILogger<ModifyAreaEmailSubjectController> logger
        )
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpPut("email/subject/{areaId}")]
        public async Task<string> Modify(
            [FromBody] SubjectText subjectText,
            string areaId,
            CancellationToken cancellationToken
        )
        {
            var curArea = await repository.GetAreaById(areaId);
            if (subjectText.Subject != curArea.Subject)
            {
                curArea.Subject = subjectText.Subject;
                await repository.CommitChangesAsync();
            }
            return subjectText.Subject;
        }
    }
}