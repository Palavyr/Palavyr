using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

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

        private string GetUriFromRequest<TRequest>()
        {
            var fieldInfo = typeof(TRequest)?.GetType().GetField("Route");
            if (fieldInfo is null) throw new Exception("Couldn't find the Uri on the request!");
            return (string)fieldInfo?.GetRawConstantValue();
        }

        public async Task<TResource> GetResource<TRequest, TResource>(CancellationToken cancellationToken)
        {
            return await Get<TRequest, TResource>(cancellationToken);
        }

        public Task<TResource> Post<TRequest, TResource>(object data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null)
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

        public async Task<TResource> Put<TRequest, TResource>(object data, CancellationToken cancellationToken)
        {
            var route = GetUriFromRequest<TRequest>();
            return await Client.PutWithContent<TResource>(route, data, cancellationToken);
        }

        public async Task Delete<TRequest>(CancellationToken cancellationToken)
        {
            var route = GetUriFromRequest<TRequest>();
            await Client.DeleteAsync(route, cancellationToken);
        }

        public async Task<bool> GetBool<TRequest>(CancellationToken cancellationToken)
        {
            return await Get<TRequest, bool>(cancellationToken);
        }

        public async Task<string> GetString<TRequest>(CancellationToken cancellationToken)
        {
            return await Get<TRequest, string>(cancellationToken);
        }

        public async Task<HttpResponseMessage> GetHttp<TRequest>(CancellationToken cancellationToken)
        {
            var uri = GetUriFromRequest<TRequest>();
            var response = await Client.GetAsync(uri, cancellationToken);
            return response;
        }

        private async Task<TResponse> Get<TRequest, TResponse>(CancellationToken cancellationToken)
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