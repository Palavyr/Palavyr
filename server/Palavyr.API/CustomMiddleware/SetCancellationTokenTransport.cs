using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.CustomMiddleware
{
    public class SetCancellationTokenTransport
    {
        private readonly RequestDelegate next;
        private ILogger<SetCancellationTokenTransport> logger;

        public SetCancellationTokenTransport(RequestDelegate next, ILogger<SetCancellationTokenTransport> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator)
        {
            await mediator.Publish(new SetCancellationTokenRequest(context.RequestAborted));
            await next(context);
        }
    }
}