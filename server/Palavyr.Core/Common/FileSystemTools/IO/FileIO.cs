using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Palavyr.Core.Common.FileSystemTools.IO
{
    public static class FileIo
    {
        public static async Task SaveFile(string filePath, IFormFile file)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();
        }

        public static async Task SaveFile(string fileName, string directoryPath, IFormFile file)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            await SaveFile(filePath, file);
        }
    }
}