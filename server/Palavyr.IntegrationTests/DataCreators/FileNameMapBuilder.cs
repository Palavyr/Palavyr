#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static class CreateFileNameMapBuilderExtensionMethod
    {
        public static FileNameMapBuilder CreateFileNameMapBuilder(this BaseIntegrationFixture test)
        {
            return new FileNameMapBuilder(test);
        }
    }

    public class FileNameMapBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string? safeName;
        private string? s3Key;
        private string? riskyName;
        private string? accountId;
        private string? areaIdentifier;

        public FileNameMapBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public FileNameMapBuilder WithSafeName(string safeName)
        {
            this.safeName = safeName;
            return this;
        }

        public FileNameMapBuilder WithS3Key(string key)
        {
            this.s3Key = key;
            return this;
        }

        public FileNameMapBuilder WithRiskyName(string name)
        {
            this.riskyName = name;
            return this;
        }

        public FileNameMapBuilder WithAreaIdentifier(string id)
        {
            this.areaIdentifier = id;
            return this;
        }
        
        public async Task<FileNameMap> Build()
        {
            var safename = this.safeName ?? A.RandomName();
            var s3Key = this.s3Key ?? A.RandomName();
            var riskyname = this.riskyName ?? A.RandomName();
            var areaId = this.areaIdentifier ?? A.RandomName();

            var resource = new FileNameMap
            {
                AccountId = test.AccountId,
                AreaIdentifier = areaId,
                RiskyName = riskyname,
                S3Key = s3Key,
                SafeName = safename
            };
            await test.DashContext.FileNameMaps.AddAsync(resource);
            await test.DashContext.SaveChangesAsync();
            return resource;
        }
    }
}