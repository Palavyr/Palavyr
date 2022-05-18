using System.Threading.Tasks;
using Autofac;
using Palavyr.API.Controllers.WidgetLive;
using Palavyr.Core.Models;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Tests.Mocks;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.WidgetLive
{
    public class SendWidgetResponseEmailControllerFixture : RealDatabaseIntegrationFixture
    {
        private readonly string Route = SendWidgetResponseEmailController.Route;

        public SendWidgetResponseEmailControllerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await this.SetupProAccount();
            await base.InitializeAsync();
        }

        [Fact]
        public async Task SendEmailWithoutPdfResponse()
        {
            // arrange 
            var intentId = A.RandomId();

            var record = await this.CreateConversationRecordBuilder().WithIntentId(intentId).Build();

            // create intent without response PDF set
            await this.CreateIntentBuilder().WithoutResponsePdf().WithIntentId(intentId).Build(); //SendPdfResponse needs to be false for this test

            var emailRequest = new EmailRequest
            {
                ConversationId = record.ConversationId,
                DynamicResponses = null!,
                EmailAddress = "test.palavyr@example.com",
                Name = "Palavyr",
                Phone = "123456"
            };

            // act
            var response = await ClientApiKey.PostWithApiKey<SendLiveEmailResultResource>(Route.Replace("{intentId}", intentId), emailRequest);

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