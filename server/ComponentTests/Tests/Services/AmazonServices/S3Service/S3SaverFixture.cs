// using System.IO;
// using System.Threading.Tasks;
// using Amazon.S3;
// using Autofac;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Configuration;
// using Palavyr.Core.Services.AmazonServices.S3Service;
// using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
// using Xunit;
// using Microsoft.Extensions.DependencyInjection;
// using IntegrationTests.AppFactory.ExtensionMethods;
// using IntegrationTests.AppFactory.IntegrationTestFixtures;
// using Shouldly;
// using Test.Common.ExtensionsMethods;
// using Test.Common.Random;
// using Xunit.Abstractions;
//
// namespace IntegrationTests.Tests.Core.Services.AmazonServices.S3Service
// {
//     public class S3SaverFixture : InMemoryIntegrationFixture, IAsyncLifetime
//     {
//         private IS3FileDeleter is3FileDeleter;
//         private IS3Downloader is3Downloader;
//         private IS3FileUploader is3FileUploader;
//         private string testUserDataBucket;
//         private string tempFile;
//         private string s3Key;
//
//         public S3SaverFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
//         {
//         }
//
//         [Fact]
//         public async Task WhenAnAttachmentFileIsUploadedToS3_ThatFileIsPresentInS3()
//         {
//             var formFile = A.RandomFormFile();
//             
//             var areaId = A.RandomName();
//             var fileName = A.RandomName();
//             s3Key = s3KeyResolver.ResolveAttachmentKey(areaId, fileName);
//
//             await is3FileUploader.StreamObjectToS3(testUserDataBucket, formFile, s3Key);
//             var result = await is3Downloader.CheckIfFileExists(testUserDataBucket, s3Key);
//             result.ShouldBe(true);
//         }
//
//         [Fact]
//         public async Task WhenTheSaverFailsToUploadAFile_AFalseValueIsReturned()
//         {
//             tempFile = Path.GetTempFileName();
//             using var stream = File.OpenRead(tempFile);
//             var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
//
//             var areaId = A.RandomName();
//             var fileName = A.RandomName();
//             s3Key = s3KeyResolver.ResolveAttachmentKey(areaId, fileName);
//
//             Should.Throw<AmazonS3Exception>(async () => await is3FileUploader.StreamObjectToS3("Palavyr-does-not-exist", formFile, s3Key));
//
//             var result = await is3Downloader.CheckIfFileExists(testUserDataBucket, s3Key);
//             result.ShouldBe(false);
//         }
//
//         public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
//         {
//             builder.AddAccountIdAndCancellationToken("123");
//             return base.CustomizeContainer(builder);
//         }
//
//         public Task InitializeAsync()
//         {
//             is3FileUploader = Container.GetService<IS3FileUploader>();
//             is3Downloader = Container.GetService<IS3Downloader>();
//             is3FileDeleter = Container.GetService<IS3FileDeleter>();
//             var config = Container.GetService<IConfiguration>();
//             testUserDataBucket = config.GetUserDataBucket();
//             return Task.CompletedTask;
//         }
//
//         public async Task DisposeAsync()
//         {
//             await is3FileDeleter.DeleteObjectFromS3Async(testUserDataBucket, s3Key);
//             File.Delete(tempFile);
//         }
//     }
// }