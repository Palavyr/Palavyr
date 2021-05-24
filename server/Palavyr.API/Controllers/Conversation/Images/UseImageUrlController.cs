﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class UseImageUrlController : PalavyrBaseController
    {
        private readonly DashContext dashContext;
        private const string Route = "images/use-link/{nodeId}";

        public UseImageUrlController(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }


        [HttpPost(Route)]
        public async Task<FileLink[]> SaveImageUrl(
            [FromHeader] string accountId,
            string nodeId,
            [FromBody] UrlRequest request,
            CancellationToken cancellationToken
        )
        {
            var image = Image.CreateImageUrlRecord(request.Url, accountId);
            dashContext.Images.Add(image);
         
            
            var node = dashContext.ConversationNodes.SingleOrDefault(x => x.NodeId == nodeId);
            if (node == null)
            {
                throw new DomainException("Duplicate node ids detected.");
            }
            node.ImageId = image.ImageId;

            await dashContext.SaveChangesAsync();
            return image.ImageUrlToFileLinks();
        }
    }

    public class UrlRequest
    {
        public string Url { get; set; }
    }
}