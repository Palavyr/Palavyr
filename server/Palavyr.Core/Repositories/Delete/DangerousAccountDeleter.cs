using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories.Delete
{
    public class UltraDangerousGlobalDeleter : DangerousAccountDeleter
    {
        private readonly IDetermineCurrentEnvironment currentEnvironment;

        public UltraDangerousGlobalDeleter(IDetermineCurrentEnvironment currentEnvironment, IFileAssetDeleter fileAssetDeleter, DashContext dashContext, ConvoContext convoContext, AccountsContext accountsContext, AccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport) : base(fileAssetDeleter, dashContext, convoContext, accountsContext, accountIdTransport, cancellationTokenTransport)
        {
            this.currentEnvironment = currentEnvironment;
        }

        public void Delete______EVERYTHING()
        {
            if (currentEnvironment.IsProduction())
            {
                throw new Exception("HOW DARE YOU TRY AND DELETE EVERYTHING IN PRODUCTION. DO NOT REMOVE THIS WARNING OR THERE WILL BE CONSEQUENCES.");
            }

            
            DeleteAllThings();

        }
    }
    
    
    public interface IDangerousAccountDeleter
    {
        Task DeleteAllThings();
    }

    public class DangerousAccountDeleter : IDangerousAccountDeleter
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly DashContext dashContext; // Try not to call these contexts directly.
        private readonly ConvoContext convoContext;
        private readonly AccountsContext accountsContext;
        private readonly AccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        private bool fileAssetsDeleted = false;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;
        private string AccountId => accountIdTransport.AccountId;

        public DangerousAccountDeleter(
            IConfigurationEntityStore<Account> accountStore,
            IFileAssetDeleter fileAssetDeleter,
            DashContext dashContext,
            ConvoContext convoContext,
            AccountsContext accountsContext,
            AccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
            this.accountStore = accountStore;
            this.fileAssetDeleter = fileAssetDeleter;
            this.dashContext = dashContext;
            this.convoContext = convoContext;
            this.accountsContext = accountsContext;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task DeleteAllThings()
        {
            await DeleteFileAssets();
            await DeleteAccountEntities();
        }

        internal virtual async Task DeleteFileAssets()
        {
            var fileAssetIds = await dashContext.FileAssets
                .Where(row => row.AccountId == AccountId)
                .Select(asset => asset.FileId)
                .ToArrayAsync(CancellationToken);
            await fileAssetDeleter.RemoveFiles(fileAssetIds);
            fileAssetsDeleted = true;
        }

        private async Task DeleteAccountEntities()
        {
            if (!fileAssetsDeleted) throw new DomainException("Cannot Delete Account without first deleting file assets.");

            var contexts = new List<DbContext>()
            {
                dashContext,
                convoContext,
                accountsContext
            };
            

            foreach (var context in contexts)
            {
                var entityTypes = context.Model.GetEntityTypes().ToList();
                foreach (var entityType in entityTypes)
                {
                    var tableContext = SetContextByEntityType(entityType);
                    var entities = await tableContext.ToListAsync(CancellationToken);

                    entities = FilterByCurrentAccount(entities);
                    
                    tableContext.RemoveRange(entities);
                }
            }
        }

        internal virtual List<IEntityType> FilterByCurrentAccount(List<IEntityType> unfiltered)
        {
            var filteredTypes = unfiltered
                .Where(x => x.GetType().GetInterfaces().Contains(typeof(IHaveAccountId))).Select(x => (IHaveAccountId)x);

            foreach (var filteredType in filteredTypes)
            {
                filteredType.includ
            }
            
            
        }

        private DbSet<IEntityType> SetContextByEntityType(IEntityType entityType)
        {
            // We need the name of generic method to call using the class reference
            var mi = GetType().GetMethod("SetTable", BindingFlags.Instance | BindingFlags.NonPublic);

            // This creates a callable MethodInfo with our generic type
            var miConstructed = mi?.MakeGenericMethod(entityType.GetType());

            // This calls the method with the generic type using Invoke
            return (DbSet<IEntityType>)miConstructed?.Invoke(this, null);
        }

        public DbSet<T> SetTable<T>(DbContext context) where T : class, IEntity
        {
            var table = context.Set<T>();
            return table;
        }
    }
}