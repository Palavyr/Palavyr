using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services
{
    public interface ILocalFileDeleter
    {
        void Delete(string filePath);
    }

    public class LocalFileDeleter : ILocalFileDeleter
    {
        private readonly ILogger<LocalFileDeleter> logger;

        public LocalFileDeleter(ILogger<LocalFileDeleter> logger)
        {
            this.logger = logger;
        }

        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                    logger.LogDebug($"Deleted local path (currently on S3). Path {filePath}");
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Unable to delete file: {filePath}");
                    logger.LogDebug(ex.Message);
                }
            }
        }
    }
}