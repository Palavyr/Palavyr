#nullable enable
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AttachmentServices
{
    public class WhenAttachmentsAreDownloaded : InMemoryIntegrationFixture, IAsyncLifetime
    {
        public WhenAttachmentsAreDownloaded(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        private string s3Key = null!;
        private string s3Bucket = null!;
        private string RiskyName = "ThisRiskyName.pdf";

        [Fact]
        public async Task PropertiesAreSetCorrectly()
        {
            var retriever = Container.GetService<IAttachmentRetriever>();
            var result = await retriever.RetrieveAttachmentFiles(IntegrationConstants.AccountId, IntegrationConstants.DefaultArea, null, default);

            // assert
            result.Length.ShouldBe(1);
            result.Select(x => x.FileNameWithExtension).Single().ShouldBe(RiskyName);
        }

        public async Task InitializeAsync()
        {
            var s3TempCreator = Container.GetService<CreateS3TempFile>();
            var s3Meta = await s3TempCreator.CreateTempFileOnS3();

            s3Key = s3Meta.Key;
            s3Bucket = s3Meta.Bucket;

            await this.CreateFileNameMapBuilder()
                .WithAccountId(IntegrationConstants.AccountId)
                .WithAreaIdentifier(IntegrationConstants.DefaultArea)
                .WithSafeName(s3Meta.LocalFileName)
                .WithS3Key(s3Meta.Key)
                .WithRiskyName(RiskyName)
                .Build();
        }

        public async Task DisposeAsync()
        {
            var s3Deleter = Container.GetService<IS3Deleter>();
            await s3Deleter.DeleteObjectFromS3Async(s3Bucket, s3Key);
        }
    }
}