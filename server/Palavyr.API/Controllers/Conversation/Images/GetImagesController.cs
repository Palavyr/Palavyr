using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class GetImagesController : PalavyrBaseController
    {
        private readonly GuidFinder guidFinder;
        private readonly DashContext dashContext;
        private const string Route = "images";

        public GetImagesController(GuidFinder guidFinder, DashContext dashContext)
        {
            this.guidFinder = guidFinder;
            this.dashContext = dashContext;
        }

        [HttpGet(Route)]
        public async Task<FileLink[]> GetImages(
            [FromHeader]
            string accountId,
            [FromQuery]
            string imageIds,
            CancellationToken cancellationToken) // should be comma separated
        {
            // TODO: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/
            List<Image> records;
            if (Request.QueryString.HasValue)
            {
                var ids = imageIds.Split(',');

                // ids should be guids
                foreach (var id in ids)
                {
                    // This throws if a GUID is not found.
                    guidFinder.FindFirstGuidSuffix(id);
                }

                records = await dashContext.Images.Where(x => x.AccountId == accountId && ids.Contains(x.ImageId)).ToListAsync(cancellationToken);
            }
            else
            {
                records = await dashContext.Images.Where(x => x.AccountId == accountId).ToListAsync(cancellationToken);
            }

            return records.ToFileLinks();
        }
    }
}