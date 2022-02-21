using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetImagesHandler : IRequestHandler<GetImagesRequest, GetImagesResponse>
    {
        private readonly GuidFinder guidFinder;
        private readonly DashContext dashContext;
        private readonly IHoldAnAccountId accountIdHolder;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetImagesHandler(GuidFinder guidFinder, DashContext dashContext, IHoldAnAccountId accountIdHolder, IHttpContextAccessor httpContextAccessor)
        {
            this.guidFinder = guidFinder;
            this.dashContext = dashContext;
            this.accountIdHolder = accountIdHolder;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetImagesResponse> Handle(GetImagesRequest request, CancellationToken cancellationToken)
        {
            // TODO: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/
            List<Image> records;
            if (httpContextAccessor.HttpContext.Request.QueryString.HasValue)
            {
                // ids should be guids
                foreach (var id in request.ImageIds)
                {
                    // This throws if a GUID is not found.
                    guidFinder.FindFirstGuidSuffix(id);
                }

                records = await dashContext
                    .Images
                    .Where(x => x.AccountId == accountIdHolder.AccountId && request.ImageIds.Contains(x.ImageId))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                records = await dashContext.Images.Where(x => x.AccountId == accountIdHolder.AccountId).ToListAsync(cancellationToken);
            }

            return new GetImagesResponse(records.ToFileLinks());
        }
    }

    public class GetImagesResponse
    {
        public GetImagesResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class GetImagesRequest : IRequest<GetImagesResponse>
    {
        public GetImagesRequest(string[] imageIds)
        {
            ImageIds = imageIds;
        }

        public string[] ImageIds { get; set; }
    }
}