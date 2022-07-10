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

        public async Task<TResource> GetResource<TRequest, TResource>(CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            return await Get<TRequest, TResource>(cancellationToken, routeFormatter);
        }


        public async Task<TResource> Post<TRequest, TResource>(CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            return await Post<TRequest, TResource>(null, cancellationToken, routeFormatter);
        }

        public async Task<TResource> Post<TRequest, TResource>(object? data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();
            if (routeFormatter != null)
            {
                route = routeFormatter(route);
            }

            if (Client.DefaultRequestHeaders.TryGetValues("test-only-apikey", out var _))
            {
                if (data is null)
                {
                    var response = await Client.PostAsyncWithoutContent(route, cancellationToken);
                    return await response.ReadResponse<TResource>();
                }

                return await Client.PostWithApiKey<TResource>(route, data, cancellationToken);
            }
            else
            {
                if (data is null)
                {
                    var response = await Client.PostAsyncWithoutContent(route, cancellationToken);
                    return await response.ReadResponse<TResource>();
                }

                return await Client.PostWithContent<TResource>(route, data, cancellationToken);
            }
        }

        public async Task<TResource> Put<TRequest, TResource>(object data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();

            if (routeFormatter != null)
            {
                route = routeFormatter(route);
            }

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

        public async Task<HttpResponseMessage> GetHttp<TRequest>(CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            var route = GetUriFromRequest<TRequest>();
            if (routeFormatter != null)
            {
                route = routeFormatter(route);
            }

            var response = await Client.GetAsync(route, cancellationToken);
            return response;
        }

        private async Task<TResponse> Get<TRequest, TResponse>(CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>
        {
            Client.Timeout = TimeSpan.FromSeconds(60);
            try
            {
                var response = await GetHttp<TRequest>(cancellationToken, routeFormatter);
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