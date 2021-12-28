using System.Threading;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public interface ITransportACancellationToken
    {
        CancellationToken CancellationToken { get; set; }
        void Assign(CancellationToken cancellationToken);
    }
    
    public class CancellationTokenTransport : ITransportACancellationToken
    {
        private CancellationToken cancellationToken;

        private bool isSet;

        public CancellationToken CancellationToken
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(CancellationToken value)
        {
            if (isSet) throw new DomainException("Cancellation token transport does not allow for resetting of cancellation tokens");

            cancellationToken = value;
            isSet = true;
            if (cancellationToken != null && cancellationToken != value) throw new DomainException("Oh no! Cancellation Token Mismatch!");
        }

        private CancellationToken Retrieve()
        {
            if (!isSet) throw new DomainException("No Cancellation Token was ever set!");
            return cancellationToken;
        }
    }
}