using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.Deletion;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    public class DeleteAreaController : PalavyrBaseController
    {
        private readonly IAreaDeleter areaDeleter;
        private IAmazonSimpleEmailService client { get; set; }

        public DeleteAreaController(
        )
        {
            this.client = client;
            this.areaDeleter = areaDeleter;
        }


        [HttpDelete("areas/delete/{areaId}")]
        public async Task Delete(
            [FromRoute]
            DeleteAreaRequest request,
            CancellationToken cancellationToken
        )
        {
            await Mediator.Send(request, cancellationToken);
        }
    }
}