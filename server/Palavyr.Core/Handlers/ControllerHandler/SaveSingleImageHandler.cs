using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveSingleImageHandler : IRequestHandler<SaveSingleImageRequest, SaveSingleImageResponse>
    {
        private readonly IImageSaver imageSaver;

        public SaveSingleImageHandler(IImageSaver imageSaver)
        {
            this.imageSaver = imageSaver;
        }

        public async Task<SaveSingleImageResponse> Handle(SaveSingleImageRequest request, CancellationToken cancellationToken)
        {
            var fileLink = await imageSaver.SaveImage(request.ImageFile, cancellationToken);
            return new SaveSingleImageResponse(new[] { fileLink });
        }
    }

    public class SaveSingleImageResponse
    {
        public SaveSingleImageResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class SaveSingleImageRequest : IRequest<SaveSingleImageResponse>
    {
        public IFormFile ImageFile { get; set; }

        public SaveSingleImageRequest(IFormFile imageFile)
        {
            ImageFile = imageFile;
        }
    }
}