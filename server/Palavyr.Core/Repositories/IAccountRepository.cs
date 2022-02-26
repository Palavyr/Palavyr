﻿using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
{
    public interface IAccountRepository
    {
        public IAccountIdTransport AccountIdTransport { get; }

        Task CommitChangesAsync();
        Task<Account> GetAccount();
        Task<Account> GetAccountOrNull();
        Task<Account> GetAccountByEmailOrNull(string emailAddress);
        Task<Account> GetAccountByEmailAddressOrNull(string emailAddress);
        Task<Session> CreateAndAddNewSession(string token, string apiKey);
        Task<Session> CreateAndAddNewSession(Account account);
        Task<Session> GetSessionOrNull(string token);
        Task RemoveSession(string sessionId);
        Task<bool> SignedStripePayloadExists(string signature);

        Task AddStripeEvent(string id, string signature);
    }
}