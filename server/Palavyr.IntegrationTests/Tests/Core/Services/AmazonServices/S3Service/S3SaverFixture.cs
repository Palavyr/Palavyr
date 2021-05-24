using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Shouldly;
using Test.Common.ExtensionsMethods;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AmazonServices.S3Service
{
    public class S3SaverFixture : InMemoryIntegrationFixture, IAsyncLifetime
    {
        private IS3Deleter s3Deleter;
        private IS3KeyResolver s3KeyResolver;
        private IS3Retriever s3Retriever;
        private IS3Saver s3Saver;
        private string testUserDataBucket;

        public S3SaverFixture(ITestOutputHelper testOutputHelper, InMemoryAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task WhenAnAttachmentFileIsUploadedToS3_ThatFileIsPresentInS3()
        {
            var tempFile = Path.GetTempFileName();
            using var stream = File.OpenRead(tempFile);
            var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));

            var accountId = A.RandomName();
            var areaId = A.RandomName();
            var fileName = A.RandomName();
            var s3Key = s3KeyResolver.ResolveAttachmentKey(accountId, areaId, fileName);

            TestOutputHelper.WriteLine("LOOK FOR THIS LIST ----------------");
            TestOutputHelper.WriteLine(Configuration.GetAccessKey());
            TestOutputHelper.WriteLine(Configuration.GetSecretKey());

            try
            {
                await s3Saver.StreamObjectToS3(testUserDataBucket, formFile, s3Key);
                var result = await s3Retriever.CheckIfFileExists(testUserDataBucket, s3Key);
                result.ShouldBe(true);
            }
            finally
            {
                await s3Deleter.DeleteObjectFromS3Async(testUserDataBucket, s3Key);
                stream.Close();
                File.Delete(tempFile);
            }
        }

        [Fact]
        public async Task WhenTheSaverFailsToUploadAFile_AFalseValueIsReturned()
        {
            
            
            var tempFile = Path.GetTempFileName();
            using var stream = File.OpenRead(tempFile);
            var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));

            var accountId = A.RandomName();
            var areaId = A.RandomName();
            var fileName = A.RandomName();
            var s3Key = s3KeyResolver.ResolveAttachmentKey(accountId, areaId, fileName);

            Should.Throw<AmazonS3Exception>(async () => await s3Saver.StreamObjectToS3("Palavyr-does-not-exist", formFile, s3Key));

            var result = await s3Retriever.CheckIfFileExists(testUserDataBucket, s3Key);
            result.ShouldBe(false);
        }

        public Task InitializeAsync()
        {
            s3Saver = Container.GetService<IS3Saver>();
            s3Retriever = Container.GetService<IS3Retriever>();
            s3KeyResolver = Container.GetService<IS3KeyResolver>();
            s3Deleter = Container.GetService<IS3Deleter>();
            testUserDataBucket = TestConfigurationExtensionMethods.GetUserDataBucket(Configuration);
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}