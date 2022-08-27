using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Test.Common.Random;

namespace Test.Common
{
    public interface ICreateS3TempFile
    {
        Task<List<TempS3FileMeta>> CreateTempFilesOnS3(int numFiles);
        Task<TempS3FileMeta> CreateTempFileOnS3();
    }

    public class CreateS3TempFile : ICreateS3TempFile
    {
        private readonly IS3FileUploader saver;
        private readonly ConfigContainerServer config;

        public CreateS3TempFile(IS3FileUploader saver, ConfigContainerServer config)
        {
            this.saver = saver;
            this.config = config;
        }

        public async Task<List<TempS3FileMeta>> CreateTempFilesOnS3(int numFiles)
        {
            var tempFiles = new List<TempS3FileMeta>();
            for (var i = 0; i < numFiles; i++)
            {
                var meta = await CreateTempFileOnS3();
                tempFiles.Add(meta);
            }

            return tempFiles;
        }

        public async Task<TempS3FileMeta> CreateTempFileOnS3()
        {
            var fileStem = A.RandomName();
            var temps3Key = TempS3Utils.CreateTempS3Key(fileStem);
            var bucket = config.AwsUserDataBucket;

            var stream = new MemoryStream(new byte[] { }, 0, 0);

            var formFile = new FormFile(stream, 0, stream.Length, "TempStream", "TempFile.tmp");
            await saver.StreamObjectToS3(bucket, formFile, temps3Key);
            return new TempS3FileMeta()
            {
                Bucket = bucket,
                Key = temps3Key,
                SafeName = fileStem + ".tmp"
            };
        }
    }

    public class TempS3FileMeta
    {
        public string Key { get; set; }
        public string Bucket { get; set; }
        public string SafeName { get; set; }
    }
}