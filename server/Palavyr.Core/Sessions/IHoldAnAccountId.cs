#nullable enable
using System;
using System.Threading;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public interface IHoldAnAccountId
    {
        public string? AccountId { get; set; }
        public void Assign(string? accountId);
    }

    public class AccountIdHolder : IHoldAnAccountId
    {
        private string? _accountId;

        public string? AccountId
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(string? accountId)
        {
            if (string.IsNullOrEmpty(_accountId))
            {
                // TODO: perhaps regex match the accountId -- we don't have type guards for the accountId being an actual accountId
                _accountId = accountId;
            }

            if (!string.IsNullOrEmpty(_accountId) && accountId != _accountId) throw new DomainException("Oh no! Autofac has crossed wires! The account should only be set one time per request!");
        }

        private string Retrieve()
        {
            if (_accountId == null) throw new DomainException("No Account Id Context Available. This is not allowed.");
            return _accountId;
        }
    }

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

    public interface ITransportACancellationToken
    {
        CancellationToken CancellationToken { get; set; }
        void Assign(CancellationToken cancellationToken);
    }
}