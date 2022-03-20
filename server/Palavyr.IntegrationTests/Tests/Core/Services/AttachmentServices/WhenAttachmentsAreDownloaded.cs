// #nullable enable
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Autofac;
// using Microsoft.Extensions.DependencyInjection;
// using Palavyr.Core.Common.UniqueIdentifiers;
// using Palavyr.Core.Models.Accounts.Schemas;
// using Palavyr.Core.Services.AmazonServices.S3Service;
// using Palavyr.Core.Services.AttachmentServices;
// using Palavyr.IntegrationTests.AppFactory;
// using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
// using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
// using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
// using Palavyr.IntegrationTests.DataCreators;
// using Shouldly;
// using Test.Common;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace Palavyr.IntegrationTests.Tests.Core.Services.AttachmentServices
// {
//     public class WhenAttachmentsAreDownloaded
//     {
//         public class WhileOnAProPlan : RealDatabaseIntegrationFixture
//         {
//             private List<TempS3FileMeta> s3Metas = null!;
//             private Account account = null!;
//             private string RiskyName => $"ThisRiskyName-{StaticGuidUtils.CreateShortenedGuid(1)}.pdf";
//
//             public WhileOnAProPlan(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
//             {
//             }
//
//             [Fact]
//             public async Task DownloadMetasLookCorrect()
//             {
//                 var retriever = Container.GetService<IAttachmentRetriever>();
//                 var downloadRequests = new List<CloudFileDownloadRequest>();
//                 foreach (var meta in s3Metas)
//                 {
//                     downloadRequests.Add(
//                         new CloudFileDownloadRequest()
//                         {
//                             FileNameWithExtension = meta.SafeName + ".pdf",
//                             LocationKey = meta.Key
//                         });
//                 }
//
//                 var result = await retriever.DownloadForAttachmentToEmail(IntegrationConstants.DefaultArea, downloadRequests, default);
//
//                 // assert
//                 result.Length.ShouldBe(5);
//             }
//
//             public override async Task InitializeAsync()
//             {
//                 account = await this.SetupProAccount();
//
//                 var s3TempCreator = Container.GetService<ICreateS3TempFile>();
//
//                 s3Metas = await s3TempCreator.CreateTempFilesOnS3(5);
//                 foreach (var s3Meta in s3Metas)
//                 {
//                     await this.CreateFileNameMapBuilder()
//                         .WithAreaIdentifier(IntegrationConstants.DefaultArea)
//                         .WithSafeName(s3Meta.SafeName)
//                         .WithS3Key(s3Meta.Key)
//                         .WithRiskyName(RiskyName)
//                         .Build();
//                 }
//
//                 await base.InitializeAsync();
//             }
//
//             public override async Task DisposeAsync()
//             {
//                 var s3Deleter = Container.GetService<IS3FileDeleter>();
//                 foreach (var s3Meta in s3Metas)
//                 {
//                     await s3Deleter.DeleteObjectFromS3Async(s3Meta.Bucket, s3Meta.Key);
//                 }
//
//                 await base.DisposeAsync();
//             }
//
//             public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
//             {
//                 builder.AddAccountIdAndCancellationToken(AccountId);
//                 return base.CustomizeContainer(builder);
//             }
//         }
//
//         public class WhileOnAFreePlan : InMemoryIntegrationFixture
//         {
//             private List<TempS3FileMeta> s3Metas = null!;
//             private string RiskyName => $"ThisRiskyName-{StaticGuidUtils.CreateShortenedGuid(1)}.pdf";
//             private Account account = null!;
//
//             public WhileOnAFreePlan(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
//             {
//             }
//
//             [Fact]
//             public async Task NoAttachmentsAreReturned()
//             {
//                 var retriever = Container.GetService<IAttachmentRetriever>();
//                 var result = await retriever.DownloadForAttachmentToEmail(IntegrationConstants.DefaultArea, new List<CloudFileDownloadRequest>(), default);
//
//                 result.Length.ShouldBe(0);
//             }
//
//             public override async Task InitializeAsync()
//             {
//                 account = await this.SetupFreeAccount();
//                 var s3TempCreator = Container.GetService<ICreateS3TempFile>();
//
//                 s3Metas = await s3TempCreator.CreateTempFilesOnS3(5);
//                 foreach (var s3Meta in s3Metas)
//                 {
//                     await this.CreateFileNameMapBuilder()
//                         .WithAreaIdentifier(IntegrationConstants.DefaultArea)
//                         .WithSafeName(s3Meta.SafeName)
//                         .WithS3Key(s3Meta.Key)
//                         .WithRiskyName(RiskyName)
//                         .Build();
//                 }
//             }
//
//             public override async Task DisposeAsync()
//             {
//                 var s3Deleter = Container.GetService<IS3FileDeleter>();
//                 foreach (var s3Meta in s3Metas)
//                 {
//                     await s3Deleter.DeleteObjectFromS3Async(s3Meta.Bucket, s3Meta.Key);
//                 }
//
//                 await base.DisposeAsync();
//             }
//
//             public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
//             {
//                 builder.AddAccountIdAndCancellationToken(AccountId);
//                 return base.CustomizeContainer(builder);
//             }
//         }
//     }
// }