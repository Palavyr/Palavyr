﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AttachmentServices
{
    public class WhenAttachmentLinksAreRetrieved : InMemoryIntegrationFixture
    {
        public WhenAttachmentLinksAreRetrieved(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task PropertiesAreSetCorrectly()
        {
            // arrange
            await this.CreateFileNameMapBuilder()
                .WithAccountId(IntegrationConstants.AccountId)
                .WithAreaIdentifier(IntegrationConstants.DefaultArea)
                .WithSafeName("safe-name.pdf")
                .WithS3Key("thisis/the/key.pdf")
                .WithRiskyName("ThisRiskyName.pdf")
                .Build();

            var retriever = Container.GetService<IAttachmentRetriever>();
            var result = await retriever.RetrieveAttachmentLinks(IntegrationConstants.DefaultArea, default);

            // assert
            result.Length.ShouldBe(1);
            result.Select(x => x.FileName).Single().ShouldBe("ThisRiskyName.pdf");
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TestLinkCreator>().As<ILinkCreator>();
            return base.CustomizeContainer(builder);
        }
    }

    public class TestLinkCreator : ILinkCreator
    {
        public string GenericCreatePreSignedUrl(string fileKey, string bucket, DateTime? expiration = null)
        {
            return "S3.ThisIsAFakePre-signedUrl";
        }
    }
}