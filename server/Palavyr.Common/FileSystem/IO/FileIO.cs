using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;



namespace Palavyr.Common.FileSystem.FormPaths.IO
{
    public static class FileIO
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
            SaveFile(filePath, file);
        }
    }
}