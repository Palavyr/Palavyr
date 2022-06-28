using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Client
{
    public interface IPalavyrClient
    {
        Task<bool> GetBool<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>;
        Task<string> GetString<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>;
        Task<TResource> GetResource<TRequest, TResource>(CancellationToken cancellationToken) where TRequest : IRequest<object>;

        Task<TResource> Post<TRequest, TResource>(object data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null) where TRequest : IRequest<object>;
        Task<TResource> Put<TRequest, TResource>(object data, CancellationToken cancellationToken) where TRequest : IRequest<object>;
        Task Delete<TRequest>(CancellationToken cancellationToken) where TRequest : IRequest<object>;
    }
}