using System.Threading;

namespace Palavyr.Core.Sessions
{
    public interface ITransportACancellationToken
    {
        CancellationToken CancellationToken { get; set; }
        void Assign(CancellationToken cancellationToken);
    }
}