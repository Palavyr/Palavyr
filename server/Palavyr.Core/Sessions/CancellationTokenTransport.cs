using System.Threading;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public class CancellationTokenTransport : ITransportACancellationToken
    {
        private CancellationToken _cancellationToken;

        private bool isSet;

        public CancellationToken CancellationToken
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(CancellationToken value)
        {
            if (isSet) throw new DomainException("Cancellation token transport does not allow for resetting of cancellation tokens");

            _cancellationToken = value;
            isSet = true;
            if (_cancellationToken != null && _cancellationToken != value) throw new DomainException("Oh no! Cancellation Token Mismatch!");
        }

        private CancellationToken Retrieve()
        {
            if (!isSet) throw new DomainException("No Cancellation Token was ever set!");
            return _cancellationToken;
        }
    }
}