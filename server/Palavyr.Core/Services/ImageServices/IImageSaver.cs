using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ImageServices
{
    public interface IImageSaver
    {
        Task<FileLink> SaveImage(string accountId, IFormFile imageFile, CancellationToken cancellationToken);
    }
}