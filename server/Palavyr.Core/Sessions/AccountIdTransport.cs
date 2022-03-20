#nullable enable
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Sessions
{
    public class AccountIdTransport : IAccountIdTransport
    {
        private string? accountId;

        public string? AccountId
        {
            get => Retrieve();
            set => Assign(value);
        }

        public void Assign(string? id)
        {
            if (string.IsNullOrEmpty(this.accountId) || this.accountId == id)
            {
                // TODO: perhaps regex match the accountId -- we don't have type guards for the accountId being an actual accountId
                this.accountId = id;
            }

            if (!string.IsNullOrEmpty(this.accountId) && id != this.accountId) throw new DomainException("Oh no! Autofac has crossed wires! The account should only be set one time per request!");
        }

        public bool IsSet()
        {
            return accountId != null;
        }

        private string Retrieve()
        {
            if (accountId == null) throw new DomainException("No Account Id Context Available. This is not allowed.");
            return accountId;
        }

        public AccountIdTransport()
        {
        }

        public AccountIdTransport(string accountId)
        {
            Assign(accountId);
        }
    }
}