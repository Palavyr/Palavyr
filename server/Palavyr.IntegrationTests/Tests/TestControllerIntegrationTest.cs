using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests
{
    public class TestControllerIntegrationTest : IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        public HttpClient AuthenticatedClient { get; set; }
        public TestControllerIntegrationTest(InMemoryWebApplicationFactory<Startup> factory)
        {
            AuthenticatedClient = factory.CreateInMemAuthedClient();
        }

        [Fact]
        public async Task RouteExists()
        {
            var response = await AuthenticatedClient.GetAsync("test");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetMediaTypeTest()
        {
            var response = await AuthenticatedClient.GetAsync("test");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task ReturnsContent()
        {
            var response = await AuthenticatedClient.GetAsync("test");
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task ReturnsExpectedJson()
        {
            var expected = "[\"One\",\"Two\",\"Three\"]";
            var response = await AuthenticatedClient.GetStringAsync("test");
            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task ReturnsExpectedObject()
        {
            var expected = new[] {"One", "Two", "Three"};
            var responseStream = await AuthenticatedClient.GetStreamAsync("test");
            var model = await JsonSerializer.DeserializeAsync<List<string>>(responseStream, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
            
            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));
        }

        [Fact]
        public async Task ReturnsExpectedResponse()
        {
            var expected = new[] {"One", "Two", "Three"};
            var model = await AuthenticatedClient.GetFromJsonAsync<List<string>>("test");
            
            Assert.NotNull(model);
            Assert.Equal(expected.OrderBy(s => s), model.OrderBy(s => s));        
        }

        [Fact]
        public async Task SetsExpectedCacheDuration()
        {
            var response = await AuthenticatedClient.GetAsync("test");
            var headers = response.Headers.CacheControl;

            Assert.True(headers.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), headers.MaxAge);
            Assert.True(headers.Public);
        }
    }
}