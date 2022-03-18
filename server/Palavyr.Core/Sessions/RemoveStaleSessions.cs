﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Stores;
using Palavyr.Core.Models.Accounts.Schemas;


namespace Palavyr.Core.Sessions
{
    public class RemoveStaleSessions : IRemoveStaleSessions
    {
        private readonly IEntityStore<Session> sessionStore;

        public RemoveStaleSessions(IEntityStore<Session> sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        public async Task CleanSessionDb(string accountId)
        {
            var now = DateTime.Now;

            var expiredSessions = await sessionStore.RawReadonlyQuery()
                .Where(sess => now >= sess.Expiration)
                .ToListAsync(sessionStore.CancellationToken);

            sessionStore.DangerousRawQuery().RemoveRange(expiredSessions);

            var previousSessions = await sessionStore.RawReadonlyQuery()
                .Where(sess => sess.AccountId == accountId)
                .ToListAsync(sessionStore.CancellationToken);

            sessionStore.DangerousRawQuery().RemoveRange(previousSessions);
        }
    }
}