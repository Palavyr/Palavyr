using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.IntegrationTests.TestUtils.ExtensionMethods;

namespace Palavyr.IntegrationTests.TestUtils
{
    public class CreateS3TempFile
    {
        private readonly IS3Saver saver;
        private readonly IConfiguration configuration;

        public CreateS3TempFile(IS3Saver saver, IConfiguration configuration)
        {
            this.saver = saver;
            this.configuration = configuration;
        }

        public async Task<TempS3FileMeta> CreateTempFileOnS3()
        {
            var fileStem = A.RandomName();
            var localTempFile = Path.GetTempFileName();
            var temps3Key = TempS3Utils.CreateTempS3Key(fileStem);
            var bucket = configuration.GetUserDataSection();
            await saver.SaveObjectToS3(bucket, localTempFile, temps3Key);
            return new TempS3FileMeta()
            {
                Bucket = bucket,
                Key = temps3Key,
                LocalFileName = fileStem + ".tmp"
            };
        }
    }

    public class TempS3FileMeta
    {
        public string Key { get; set; }
        public string Bucket { get; set; }
        public string LocalFileName { get; set; }
    }
}