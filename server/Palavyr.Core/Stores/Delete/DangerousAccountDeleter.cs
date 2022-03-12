using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores.Delete
{
    public class UltraDangerousGlobalDeleter : DangerousAccountDeleter
    {
        private readonly IDetermineCurrentEnvironment currentEnvironment;

        public UltraDangerousGlobalDeleter(
            IEntityStore<Account> accountStore,
            IDetermineCurrentEnvironment currentEnvironment,
            IFileAssetDeleter fileAssetDeleter,
            DashContext dashContext,
            ConvoContext convoContext,
            AccountsContext accountsContext,
            AccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport) : base(fileAssetDeleter, dashContext, convoContext, accountsContext, accountIdTransport, cancellationTokenTransport)
        {
            this.currentEnvironment = currentEnvironment;
        }

        public async Task Delete______EVERYTHING()
        {
            if (currentEnvironment.IsProduction())
            {
                throw new Exception("HOW DARE YOU TRY AND DELETE EVERYTHING IN PRODUCTION. DO NOT REMOVE THIS WARNING OR THERE WILL BE CONSEQUENCES.");
            }

            await DeleteAllThings();
        }
    }


    public interface IDangerousAccountDeleter
    {
        Task DeleteAllThings();
    }

    public class DangerousAccountDeleter : IDangerousAccountDeleter
    {
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly DashContext dashContext; // Try not to call these contexts directly.
        private readonly ConvoContext convoContext;
        private readonly AccountsContext accountsContext;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        private bool fileAssetsDeleted = false;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;
        private string AccountId => accountIdTransport.AccountId;

        public DangerousAccountDeleter(
            IFileAssetDeleter fileAssetDeleter,
            DashContext dashContext,
            ConvoContext convoContext,
            AccountsContext accountsContext,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
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
                    var tableContext = SetContextByEntityType(entityType, context);
                    var includePaths = tableContext.GetIncludePaths(context, entityType.ClrType);
                    var content = await tableContext.Include(includePaths).ToListAsync(CancellationToken);
                    content = FilterByCurrentAccount(content);
                    tableContext.RemoveRange(content);
                }
            }
        }

        internal virtual List<IEntity> FilterByCurrentAccount(List<IEntity> unfiltered)
        {
            var entities = new List<IEntity>();
            try
            {
                var selected = unfiltered.Select(x => (IHaveAccountId)x);
                foreach (var select in selected)
                {
                    if (select.AccountId == AccountId)
                    {
                        entities.Add((IEntity)select);
                    }
                }

                return entities;
            }
            catch (Exception)
            {
                entities.Clear();
                return entities;
            }
        }

        private DbSet<IEntity> SetContextByEntityType(IEntityType entityType, DbContext context)
        {
            // We need the name of generic method to call using the class reference
            var mi = GetType().GetMethod("SetTable", BindingFlags.Instance | BindingFlags.NonPublic);

            // This creates a callable MethodInfo with our generic type
            var miConstructed = mi?.MakeGenericMethod(entityType.ClrType);

            // This calls the method with the generic type using Invoke
            var constructed = (DbSet<IEntity>)miConstructed?.Invoke(this, new object[] { context });
            return constructed;
        }

        private DbSet<T> SetTable<T>(DbContext context) where T : class
        {
            var table = context.Set<T>();
            return table;
        }
    }
}