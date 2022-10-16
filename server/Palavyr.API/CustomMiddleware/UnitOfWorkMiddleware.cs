using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Stores;

namespace Palavyr.API.CustomMiddleware
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<UnitOfWorkMiddleware> logger;

        private Dictionary<string, string> ResponseHeaders = new()
        {
            { "Access-Control-Allow-Origin", "*" }
        };

        public UnitOfWorkMiddleware(RequestDelegate next, ILogger<UnitOfWorkMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {

            await next(context);

            await unitOfWorkContextProvider.CloseUnitOfWork();
        }
    }
}