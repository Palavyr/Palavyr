using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.CustomMiddleware
{
    public class SetCancellationTokenTransportMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<SetCancellationTokenTransportMiddleware> logger;

        public SetCancellationTokenTransportMiddleware(RequestDelegate next, ILogger<SetCancellationTokenTransportMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator, ILifetimeScope lifetimeScope)
        {
            await mediator.Publish(new SetCancellationTokenRequest(context.RequestAborted));
            await next(context);
        }
    }
}