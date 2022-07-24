using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
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
            // create intent without response PDF set
            var intent = await this.CreateIntentBuilder()
                .WithoutResponsePdf()
                .Build(); //SendPdfResponse needs to be false for this test

            var record = await this.CreateConversationRecordBuilder().WithIntentId(intent.IntentId).Build();
            
            var emailRequest = new EmailRequest
            {
                ConversationId = record.ConversationId,
                PricingStrategyResponses = new PricingStrategyResponses(),
                EmailAddress = "test.palavyr@example.com",
                Name = "Palavyr",
                Phone = "123456"
            };

            // act
            var response = await ApikeyClient.Post<SendWidgetResponseEmailRequest, SendLiveEmailResultResource>(emailRequest, CancellationToken, s => s.Replace("{intentId}", intent.IntentId));

            // assert
            response.NextNodeId.ShouldBe(EndingSequenceAttacher.EmailSuccessfulNodeId);
            response.Result.ShouldBeTrue();
            response.FileAsset.ShouldBeNull();
        }
    }
}