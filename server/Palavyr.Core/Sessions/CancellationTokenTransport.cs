using System.Threading;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public class CancellationTokenTransport : ICancellationTokenTransport
    {
        private CancellationToken cancellationToken;

        private bool isSet;
        
        public bool IsSet()
        {
            return isSet;
        }

        public CancellationToken CancellationToken
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(CancellationToken value)
        {
            if (isSet)
            {
                throw new DomainException("Cancellation token transport does not allow for resetting of cancellation tokens");
            }

            var newId = new GuidUtils().CreateNewId();
            id = newId;
            cancellationToken = value;
            isSet = true;
            if (cancellationToken != null && cancellationToken != value) throw new DomainException("Oh no! Cancellation Token Mismatch!");
        }
        private string id;

        public CancellationTokenTransport(CancellationToken cancellationToken)
        {
            Assign(cancellationToken);
        }

        public CancellationTokenTransport()
        {
        }

        private CancellationToken Retrieve()
        {
            if (!isSet) throw new DomainException("No Cancellation Token was ever set!");
            return cancellationToken;
        }
    }
}