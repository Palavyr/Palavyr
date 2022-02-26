using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using NSubstitute;
using Palavyr.API.Controllers.WidgetLive;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Tests.Mocks;
using Shouldly;
using Test.Common.Builders;
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
        }

        [Fact]
        public async Task SendEmailWithoutPdfResponse()
        {
            // arrange 
            var intentId = A.RandomId();
            var recordBuilder = this.CreateConversationRecordBuilder();
            var record = await recordBuilder.WithIntentId(intentId).Build();

            // create intent without response PDF set
            var intentBuilder = this.CreateIntentBuilder();
            var intent = await intentBuilder.WithoutResponsePdf().WithIntentId(intentId).Build(); //SendPdfResponse needs to be false for this test

            var emailRequest = new EmailRequest
            {
                ConversationId = record.ConversationId,
                DynamicResponses = null,
                EmailAddress = "test.palavyr@example.com",
                Name = "Palavyr",
                Phone = "123456"
            };

            // act
            var response = await ClientApiKey.PostWithApiKey<SendEmailResultResponse>(Route.Replace("{intentId}", intentId), emailRequest);
            
            // assert
            response.NextNodeId.ShouldBe(EndingSequenceAttacher.EmailSuccessfulNodeId);
            response.Result.ShouldBeTrue();
            response.PdfLink.ShouldBeNull();
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>();
            return base.CustomizeContainer(builder);
        }

    }
}