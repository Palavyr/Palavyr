using System.Threading;

namespace Palavyr.Core.Sessions
{
    public interface ICancellationTokenTransport
    {
        CancellationToken CancellationToken { get; set; }
        void Assign(CancellationToken cancellationToken);
    }
}