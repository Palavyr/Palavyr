﻿using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Stores.StoreExtensionMethods
{
    public static class AccountStoreExtensionMethods
    {
        public static async Task<Account> GetAccount(this IEntityStore<Account> accountStore)
        {
            return await accountStore.Get(accountStore.AccountId, s => s.AccountId);
        }
    }
}