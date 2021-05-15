using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Common.FileSystemTools
{
    public interface ILocalIo
    {
        Task SaveFile(string fullPath, IFormFile file);
    }

    public class LocalIo : ILocalIo
    {
        private readonly ILogger<LocalIo> logger;

        public LocalIo(ILogger<LocalIo> logger)
        {
            this.logger = logger;
        }
        public async Task SaveFile(string fullPath, IFormFile file)
        {
            try
            {

                await using var fileStream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
                fileStream.Close();
            }
            catch (IOException)
            {
                logger.LogDebug($"Unable to write file to disk: {fullPath}!");
                throw;
            }
        }
    }
}