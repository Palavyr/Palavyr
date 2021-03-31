using System;
using System.Linq;
using Palavyr.Core.Data;

namespace Palavyr.Core.BackgroundJobs
{
    public class RemoveStaleSessions : IRemoveStaleSessions
    {
        private readonly AccountsContext Context;
        
        public RemoveStaleSessions(AccountsContext context)
        {
            Context = context;
        }

        public void CleanSessionDB()
        {
            var now = DateTime.Now;

            var expiredSessions = Context.Sessions.Where(sess => now >= sess.Expiration);
            Context.Sessions.RemoveRange(expiredSessions);
            Context.SaveChanges();
        }

    }
}