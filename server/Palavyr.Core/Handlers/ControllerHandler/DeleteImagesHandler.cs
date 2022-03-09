﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteImagesHandler : IRequestHandler<DeleteImagesRequest, DeleteImagesResponse>
    {
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly GuidFinder guidFinder;
        private readonly IHttpContextAccessor accessor;

        public DeleteImagesHandler(IFileAssetDeleter fileAssetDeleter, GuidFinder guidFinder, IHttpContextAccessor accessor)
        {
            this.fileAssetDeleter = fileAssetDeleter;
            this.guidFinder = guidFinder;
            this.accessor = accessor;
        }

        public async Task<DeleteImagesResponse> Handle(DeleteImagesRequest request, CancellationToken cancellationToken)
        {
            // TODO: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/
            if (!accessor.HttpContext.Request.QueryString.HasValue)
            {
                throw new DomainException("Image deletion failed. No image id was provided.");
            }


            // ids should be guids
            foreach (var id in request.ImageIds)
            {
                // This throws if a GUID is not found.
                guidFinder.FindFirstGuidSuffixOrNull(id);
            }

            var fileLinks = await fileAssetDeleter.RemoveFiles(request.ImageIds);
            return new DeleteImagesResponse(fileLinks);
        }
    }

    public class DeleteImagesResponse
    {
        public DeleteImagesResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class DeleteImagesRequest : IRequest<DeleteImagesResponse>
    {
        public DeleteImagesRequest(string[] imageIds)
        {
            ImageIds = imageIds;
        }

        public string[] ImageIds { get; set; }
    }
}