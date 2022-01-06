#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AttachmentServices
{
    public class WhenAttachmentsAreDownloaded
    {
        public class WhileOnAProPlan : ProPlanIntegrationFixture
        {
            private List<TempS3FileMeta> s3Metas = null!;
            private string RiskyName => $"ThisRiskyName-{StaticGuidUtils.CreateShortenedGuid(1)}.pdf";

            public WhileOnAProPlan(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task DownloadMetasLookCorrect()
            {
                var retriever = Container.GetService<IAttachmentRetriever>();
                var downloadRequests = new List<S3SDownloadRequestMeta>();
                foreach (var meta in s3Metas)
                {
                    downloadRequests.Add(
                        new S3SDownloadRequestMeta()
                        {
                            FileNameWithExtension = meta.SafeName + ".pdf",
                            S3Key = meta.Key
                        });
                }

                var result = await retriever.RetrieveAttachmentFiles(IntegrationConstants.DefaultArea, downloadRequests, default);

                // assert
                result.Length.ShouldBe(5);
            }

            public override async Task InitializeAsync()
            {
                var s3TempCreator = Container.GetService<ICreateS3TempFile>();

                s3Metas = await s3TempCreator.CreateTempFilesOnS3(5);
                foreach (var s3Meta in s3Metas)
                {
                    await this.CreateFileNameMapBuilder()
                        .WithAccountId(IntegrationConstants.AccountId)
                        .WithAreaIdentifier(IntegrationConstants.DefaultArea)
                        .WithSafeName(s3Meta.SafeName)
                        .WithS3Key(s3Meta.Key)
                        .WithRiskyName(RiskyName)
                        .Build();
                }

                await base.InitializeAsync();
            }

            public override async Task DisposeAsync()
            {
                var s3Deleter = Container.GetService<IS3Deleter>();
                foreach (var s3Meta in s3Metas)
                {
                    await s3Deleter.DeleteObjectFromS3Async(s3Meta.Bucket, s3Meta.Key);
                }

                await base.DisposeAsync();
            }

            public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
            {
                builder.AddAccountIdAndCancellationToken();
                return base.CustomizeContainer(builder);
            }
        }

        public class WhileOnAFreePlan : FreePlanIntegrationFixture
        {
            private List<TempS3FileMeta> s3Metas = null!;
            private string RiskyName => $"ThisRiskyName-{StaticGuidUtils.CreateShortenedGuid(1)}.pdf";

            public WhileOnAFreePlan(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task NoAttachmentsAreReturned()
            {
                var retriever = Container.GetService<IAttachmentRetriever>();
                var result = await retriever.RetrieveAttachmentFiles(IntegrationConstants.DefaultArea, new List<S3SDownloadRequestMeta>(), default);

                // assert
                result.Length.ShouldBe(0);
            }

            public override async Task InitializeAsync()
            {
                var s3TempCreator = Container.GetService<ICreateS3TempFile>();

                s3Metas = await s3TempCreator.CreateTempFilesOnS3(5);
                foreach (var s3Meta in s3Metas)
                {
                    await this.CreateFileNameMapBuilder()
                        .WithAccountId(IntegrationConstants.AccountId)
                        .WithAreaIdentifier(IntegrationConstants.DefaultArea)
                        .WithSafeName(s3Meta.SafeName)
                        .WithS3Key(s3Meta.Key)
                        .WithRiskyName(RiskyName)
                        .Build();
                }

                await base.InitializeAsync();
            }

            public override async Task DisposeAsync()
            {
                var s3Deleter = Container.GetService<IS3Deleter>();
                foreach (var s3Meta in s3Metas)
                {
                    await s3Deleter.DeleteObjectFromS3Async(s3Meta.Bucket, s3Meta.Key);
                }

                await base.DisposeAsync();
            }

            public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
            {
                builder.AddAccountIdAndCancellationToken();
                return base.CustomizeContainer(builder);
            }
        }
    }
}