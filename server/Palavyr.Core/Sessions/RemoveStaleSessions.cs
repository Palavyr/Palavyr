using System;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data;

namespace Palavyr.Core.Sessions
{
    public class RemoveStaleSessions : IRemoveStaleSessions
    {
        private readonly AccountsContext context;
        
        public RemoveStaleSessions(AccountsContext context)
        {
            this.context = context;
        }

        public async Task CleanSessionDb()
        {
            var now = DateTime.Now;

            var expiredSessions = context.Sessions.Where(sess => now >= sess.Expiration);
            context.Sessions.RemoveRange(expiredSessions);
            await context.SaveChangesAsync();
        }

    }
}