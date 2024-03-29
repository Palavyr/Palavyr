﻿// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Autofac;
// using Palavyr.Core.Services.AmazonServices;
// using Palavyr.Core.Services.AttachmentServices;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace Palavyr.Component.Tests.Services.AttachmentServices
// {
//     public class WhenAttachmentLinksAreRetrieved : InMemoryIntegrationFixture
//     {
//         public WhenAttachmentLinksAreRetrieved(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
//         {
//         }
//
//         [Fact]
//         public async Task PropertiesAreSetCorrectly()
//         {
//             // arrange
//             await this.CreateFileNameMapBuilder()
//                 .WithIntentId(IntegrationConstants.DefaultIntent)
//                 .WithSafeName("safe-name.pdf")
//                 .WithS3Key("thisis/the/key.pdf")
//                 .WithRiskyName("ThisRiskyName.pdf")
//                 .Build();
//
//             var retriever = Container.GetService<IAttachmentRetriever>();
//             var result = await retriever.GetAttachmentLinksForIntent(IntegrationConstants.DefaultIntent);
//
//             // assert
//             result.Length.ShouldBe(1);
//             result.Select(x => x.FileName).Single().ShouldBe("ThisRiskyName.pdf");
//         }
//
//         public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
//         {
//             builder.RegisterType<TestLinkCreator>().As<ILinkCreator>();
//             return base.CustomizeContainer(builder);
//         }
//     }
//
//     public class TestLinkCreator : ILinkCreator
//     {
//         public string GenericCreatePreSignedUrl(string fileKey, string bucket, DateTime? expiration = null)
//         {
//             return "S3.ThisIsAFakePre-signedUrl";
//         }
//
//         public Task<string> CreateLink(string fileAssetId)
//         {
//             throw new NotImplementedException();
//         }
//
//         public Task<string[]> CreateManyLinks(string[] fileAssetIds)
//         {
//             throw new NotImplementedException();
//         }
//
//         public Task<string[]> CreateManyLinks(IEnumerable<string> fileAssetIds)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }