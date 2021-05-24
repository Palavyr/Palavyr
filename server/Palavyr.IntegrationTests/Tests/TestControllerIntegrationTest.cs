using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class TestControllerIntegrationTest : InMemoryIntegrationFixture
    {

        public TestControllerIntegrationTest(ITestOutputHelper testOutputHelper, InMemoryAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
        
        [Fact]
        public async Task HomeExists()
        {
            var response = await Client.GetAsync("");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RouteExists()
        {
            var response = await Client.GetAsync("test");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetMediaTypeTest()
        {
            var response = await Client.GetAsync("test");
            Assert.Equal("application/json", response?.Content?.Headers?.ContentType?.MediaType);
        }

        [Fact]
        public async Task ReturnsContent()
        {
            var response = await Client.GetAsync("test");
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task ReturnsExpectedJson()
        {
            var expected = "[\"One\",\"Two\",\"Three\"]";
            var response = await Client.GetStringAsync("test");
            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task ReturnsExpectedObject()
        {
            var expected = new[] {"One", "Two", "Three"};
            var responseStream = await Client.GetStreamAsync("test");
            var model = await JsonSerializer.DeserializeAsync<List<string>>(responseStream, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
            
            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));
        }

        [Fact]
        public async Task ReturnsExpectedResponse()
        {
            var expected = new[] {"One", "Two", "Three"};
            var model = await Client.GetFromJsonAsync<List<string>>("test");
            
            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));        
        }

        [Fact]
        public async Task SetsExpectedCacheDuration()
        {
            var response = await Client.GetAsync("test");
            var headers = response.Headers.CacheControl;

            Assert.True(headers.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), headers.MaxAge);
            Assert.True(headers.Public);
        }
    }
}