using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Client
{
    public class PalavyrClient : IPalavyrClient
    {
        public readonly HttpClient Client;

        public PalavyrClient(HttpClient client)
        {
            this.Client = client;
            DefaultRequestHeaders = client.DefaultRequestHeaders;
        }

        public HttpRequestHeaders DefaultRequestHeaders { get; set; }

        private string GetUriFromRequest<TRequest>() where TRequest : IRequest<object>
        {
            var fieldInfos = typeof(TRequest).GetFields(
                BindingFlags.Public |
                BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var route = (string)fieldInfos
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .Single(x => x.Name == "Route")
                .GetRawConstantValue();

            return route;
        }

        public async Task<TResource> GetResource<TRequest, TResource>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            return await Get<TRequest, TResource>(cancellationToken);
        }

        public Task<TResource> Post<TRequest, TResource>(object data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();
            if (routeFormatter != null)
            {
                route = routeFormatter(route);
            }

            if (Client.DefaultRequestHeaders.TryGetValues("test-only-apikey", out var _))
            {
                return Client.PostWithApiKey<TResource>(route, data, cancellationToken);
            }
            else
            {
                return Client.PostWithContent<TResource>(route, data, cancellationToken);
            }
        }

        public async Task<TResource> Put<TRequest, TResource>(object data, CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();
            return await Client.PutWithContent<TResource>(route, data, cancellationToken);
        }

        public async Task Delete<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();
            await Client.DeleteAsync(route, cancellationToken);
        }

        public async Task<bool> GetBool<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            return await Get<TRequest, bool>(cancellationToken);
        }

        public async Task<string> GetString<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            return await Get<TRequest, string>(cancellationToken);
        }

        public async Task<HttpResponseMessage> GetHttp<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            var uri = GetUriFromRequest<TRequest>();
            var response = await Client.GetAsync(uri, cancellationToken);
            return response;
        }

        private async Task<TResponse> Get<TRequest, TResponse>(CancellationToken cancellationToken) where TRequest : IRequest<object>
        {
            Client.Timeout = TimeSpan.FromSeconds(60);
            try
            {
                var response = await GetHttp<TRequest>(cancellationToken);
                response.EnsureSuccessStatusCode();

                var result = await response.ReadResponse<TResponse>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}