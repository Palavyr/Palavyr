using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public class StripeClientDecorator : IStripeClient
    {
        private readonly IStripeClient inner;
        private readonly ILogger<StripeClientDecorator> logger;
        private readonly IDetermineCurrentEnvironment currentEnvironment;

        public StripeClientDecorator(IStripeClient inner, ILogger<StripeClientDecorator> logger, IDetermineCurrentEnvironment currentEnvironment)
        {
            this.inner = inner;
            this.logger = logger;
            this.currentEnvironment = currentEnvironment;
        }

        public async Task<T> RequestAsync<T>(HttpMethod method, string path, BaseOptions options, RequestOptions requestOptions, CancellationToken cancellationToken = new CancellationToken()) where T : IStripeEntity
        {
            try
            {
                return await inner.RequestAsync<T>(method, path, options, requestOptions, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (currentEnvironment.IsProduction() || currentEnvironment.IsStaging())
                {
                    throw new DomainException("Failed to contact Stripe. Please contact info.palavyr@gmail.com. Stripe is probably not responding.");
                }
                else
                {
                    var message = @"
                    Failed to reach the Stripe Web Servers.
                    You need to start the Stripe Container in order to run the application. We don't use real stripe in dev.
                    To run the mock server, install docker and run  `docker run --rm -it -p 12111-12112:12111-12112 stripemock/stripe-mock:latest`
                    ";
                    logger.LogError(message);
                    throw new DomainException(message);
                }
            }
        }

        public Task<Stream> RequestStreamingAsync(HttpMethod method, string path, BaseOptions options, RequestOptions requestOptions, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public string ApiBase => inner.ApiBase;
        public string ApiKey => inner.ApiKey;
        public string ClientId => inner.ClientId;
        public string ConnectBase => inner.ConnectBase;
        public string FilesBase => inner.FilesBase;
    }
}