using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers
{
    [Obsolete]
    public class UseImageUrlHandler : IRequestHandler<UseImageUrlRequest, UseImageUrlResponse>
    {
        private readonly DashContext dashContext;
        private readonly IHoldAnAccountId accountIdHolder;

        public UseImageUrlHandler(DashContext dashContext, IHoldAnAccountId accountIdHolder)
        {
            this.dashContext = dashContext;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<UseImageUrlResponse> Handle(UseImageUrlRequest request, CancellationToken cancellationToken)
        {
            var image = Image.CreateImageUrlRecord(request.Url, accountIdHolder.AccountId);
            dashContext.Images.Add(image);

            var node = dashContext.ConversationNodes.SingleOrDefault(x => x.NodeId == request.NodeId);
            if (node == null)
            {
                throw new DomainException("Duplicate node ids detected.");
            }

            node.ImageId = image.ImageId;

            await dashContext.SaveChangesAsync();
            return new UseImageUrlResponse(image.ImageUrlToFileLinks());
        }
    }

    public class UseImageUrlResponse
    {
        public UseImageUrlResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class UseImageUrlRequest : IRequest<UseImageUrlResponse>
    {
        public string NodeId { get; set; }
        public string Url { get; set; }

        public UseImageUrlRequest(string nodeId, string url)
        {
            NodeId = nodeId;
            Url = url;
        }
    }
}