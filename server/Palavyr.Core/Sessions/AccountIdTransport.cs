#nullable enable
using System;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public interface IHoldAnAccountId
    {
        public string? AccountId { get; set; }
        public void Assign(string? accountId);
    }

    public class AccountIdTransport : IHoldAnAccountId
    {
        private string? accountId;

        public string? AccountId
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(string? accountId)
        {
            if (string.IsNullOrEmpty(this.accountId))
            {
                // TODO: perhaps regex match the accountId -- we don't have type guards for the accountId being an actual accountId
                this.accountId = accountId;
            }

            if (!string.IsNullOrEmpty(this.accountId) && accountId != this.accountId) throw new DomainException("Oh no! Autofac has crossed wires! The account should only be set one time per request!");
        }

        private string Retrieve()
        {
            if (accountId == null) throw new DomainException("No Account Id Context Available. This is not allowed.");
            return accountId;
        }
    }
}