using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Handlers.Pipelines
{
    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TRequest>, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!validators.ToList().Any()) return await next();
            // if (!EnumerableExtensions.Any(validators)) 
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = results
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToArray();
            if (!errors.Any()) return await next();

            throw new MultiMessageDomainException("Validation Failed", errors.Select(x => x.ErrorMessage).ToArray());
        }
    }
}