using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Palavyr.Core.Validators
{
    public class RequestHandlerValidationDecorator<TEvent, TResponse> : IRequestHandler<TEvent, TResponse> where TEvent : IRequest<TResponse>
    {
        private readonly ILogger logger;
        private readonly IRequestHandler<TEvent, TResponse> inner;
        private readonly IRequestValidator<TEvent, TResponse> validator;

        public RequestHandlerValidationDecorator(ILogger logger, IRequestHandler<TEvent, TResponse> inner, IRequestValidator<TEvent, TResponse> validator)
        {
            this.logger = logger;
            this.inner = inner;
            this.validator = validator;
        }

        public async Task<TResponse> Handle(TEvent request, CancellationToken cancellationToken)
        {
            logger.Debug($"Validating: {typeof(TEvent)}");
            await validator.Validate(request);
            var response = await inner.Handle(request, cancellationToken);
            return response;
        }
    }
}