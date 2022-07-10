using System.Threading.Tasks;
using Autofac;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Mocks;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.WidgetLive
{
    public class SendWidgetResponseEmailControllerFixture : IntegrationTest
    {
        public SendWidgetResponseEmailControllerFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task SendEmailWithoutPdfResponse()
        {
            // arrange 
            var intentId = A.RandomId();

            var record = await this.CreateConversationRecordBuilder().WithIntentId(intentId).Build();

            // create intent without response PDF set
            await this.CreateIntentBuilder()
                .WithoutResponsePdf()
                .WithIntentId(intentId)
                .Build(); //SendPdfResponse needs to be false for this test

            var emailRequest = new EmailRequest
            {
                ConversationId = record.ConversationId,
                DynamicResponses = new DynamicResponses(),
                EmailAddress = "test.palavyr@example.com",
                Name = "Palavyr",
                Phone = "123456"
            };

            // act
            var response = await ApikeyClient.Post<SendWidgetResponseEmailRequest, SendLiveEmailResultResource>(emailRequest, CancellationToken, s => s.Replace("{intentId}", intentId));

            // assert
            response.NextNodeId.ShouldBe(EndingSequenceAttacher.EmailSuccessfulNodeId);
            response.Result.ShouldBeTrue();
            response.FileAsset.ShouldBeNull();
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>();
            return base.CustomizeContainer(builder);
        }
    }
}