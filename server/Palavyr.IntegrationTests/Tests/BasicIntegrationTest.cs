using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Palavyr.API.Controllers.Testing;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class BasicIntegrationTest : IntegrationTest
    {
        public BasicIntegrationTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task HomeExists()
        {
            var response = await Client.GetHttp<TestRequest>(CancellationToken);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RouteExists()
        {
            var response = await Client.GetHttp<TestRequest>(CancellationToken);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetMediaTypeTest()
        {
            var response = await Client.GetHttp<TestRequest>(CancellationToken);
            Assert.Equal("application/json", response?.Content?.Headers?.ContentType?.MediaType);
        }

        [Fact]
        public async Task ReturnsContent()
        {
            var response = await Client.GetHttp<TestRequest>(CancellationToken);
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task ReturnsExpectedJson()
        {
            var expected = "[\"One\",\"Two\",\"Three\"]";
            var response = await Client.GetHttp<TestRequest>(CancellationToken);

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ReturnsExpectedObject()
        {
            var expected = new[] { "One", "Two", "Three" };
            var responseStream = await Client.Client.GetStreamAsync(TestRequest.Route);
            var model = await JsonSerializer.DeserializeAsync<List<string>>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));
        }

        [Fact]
        public async Task ReturnsExpectedResponse()
        {
            var expected = new[] { "One", "Two", "Three" };
            var model = await Client.Client.GetFromJsonAsync<List<string>>("test");

            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));
        }

        [Fact]
        public async Task SetsExpectedCacheDuration()
        {
            var response = await Client.Client.GetAsync("test");
            var headers = response.Headers.CacheControl;

            Assert.True(headers.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), headers.MaxAge);
            Assert.True(headers.Public);
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            WebHostFactory.Dispose();
        }
    }
}