using System;
using System.Threading;
using System.Threading.Tasks;

namespace Palavyr.Client
{
    public interface IPalavyrClient
    {
        Task<bool> GetBool<TRequest>(CancellationToken cancellationToken);
        Task<string> GetString<TRequest>(CancellationToken cancellationToken);
        Task<TResource> GetResource<TRequest, TResource>(CancellationToken cancellationToken);

        Task<TResource> Post<TRequest, TResource>(object data, CancellationToken cancellationToken, Func<string, string>? routeFormatter = null);
        Task<TResource> Put<TRequest, TResource>(object data, CancellationToken cancellationToken);
        Task Delete<TRequest>(CancellationToken cancellationToken);
    }
}