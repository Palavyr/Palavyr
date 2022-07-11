
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Stores.Delete
{
    public class UltraDangerousGlobalDeleter : DangerousAccountDeleter
    {
        private readonly IDetermineCurrentEnvironment currentEnvironment;

        public UltraDangerousGlobalDeleter(
            IServiceProvider serviceProvider,
            IUnitOfWorkContextProvider unitOfWorkContextProvider,
            IDetermineCurrentEnvironment currentEnvironment,
            IFileAssetDeleter fileAssetDeleter,
            DashContext dashContext,
            ConvoContext convoContext,
            AccountsContext accountsContext,
            AccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
            : base(
                serviceProvider,
                unitOfWorkContextProvider, fileAssetDeleter, dashContext, convoContext, accountsContext, accountIdTransport, cancellationTokenTransport)
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
        private readonly IServiceProvider lifetimeScope;
        private readonly IUnitOfWorkContextProvider contextProvider;
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly DashContext dashContext; // Try not to call these contexts directly.
        private readonly AccountsContext accountsContext;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly List<IEntityType> allEntities = new List<IEntityType>();
        private bool fileAssetsDeleted = false;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;
        private string AccountId => accountIdTransport.AccountId;

        public DangerousAccountDeleter(
            IServiceProvider lifetimeScope,
            IUnitOfWorkContextProvider contextProvider,
            IFileAssetDeleter fileAssetDeleter,
            DashContext dashContext,
            ConvoContext convoContext,
            AccountsContext accountsContext,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
            this.lifetimeScope = lifetimeScope;
            this.contextProvider = contextProvider;
            this.fileAssetDeleter = fileAssetDeleter;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;

            allEntities.AddRange(dashContext.Model.GetEntityTypes().ToList());
            allEntities.AddRange(accountsContext.Model.GetEntityTypes().ToList());
            allEntities.AddRange(convoContext.Model.GetEntityTypes().ToList());
        }

        public async Task DeleteAllThings()
        {
            await DeleteFileAssets();
            await DeleteAccountEntities();
        }

        internal virtual async Task DeleteFileAssets()
        {
            var fileAssetIds = await dashContext
                .FileAssets
                .Where(row => row.AccountId == AccountId)
                .Select(asset => asset.FileId)
                .ToArrayAsync(CancellationToken);
            await fileAssetDeleter.RemoveFiles(fileAssetIds);
            fileAssetsDeleted = true;
        }

        private int Counter = 0;

        private async Task DeleteAccountAt<TContext, TEntity>() where TEntity : class, IEntity where TContext : DbContext
        {
            if (!allEntities.Select(x => x.ClrType).Contains(typeof(TEntity)))
            {
                return;
            }

            if (!typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                return;
            }

            if (typeof(TEntity).GetInterfaces().Contains(typeof(IHaveAccountId)))
            {
                var newContext = lifetimeScope.GetService<TContext>();

                var store = newContext.Set<TEntity>();
                var entities = await store.ToListAsync(CancellationToken);

                var toDelete = FilterByCurrentAccount(entities);
                store.RemoveRange(toDelete);
                await contextProvider.DangerousCommitAllContexts();
                Counter += 1;
            }
        }

        private async Task DeleteAccountEntities()
        {
            if (!fileAssetsDeleted) throw new DomainException("Cannot Delete Account without first deleting file assets.");
            
            var accountStore = lifetimeScope.GetService<IEntityStore<Account>>();
            // var account = await accountsContext.Accounts.SingleOrDefaultAsync(x => x.AccountId == accountStore?.AccountId, accountStore?.CancellationToken ?? new CancellationToken());
            
            var account = await accountStore.GetAccount();
            var stripeCustomerId = account.StripeCustomerId;

            // ORDER MATTERS HERE SINCE WE LINK TABLES IN THE ORM AND WE DONT USE REFLECTION TO AUTO-INCLUDE ALL CHILD ENTITIES from the aggregate root
            await DeleteAccountAt<DashContext, ConversationNode>();
            await DeleteAccountAt<DashContext, DynamicTableMeta>();
            await DeleteAccountAt<DashContext, FileAsset>();
            await DeleteAccountAt<DashContext, WidgetPreference>();

            await DeleteAccountAt<DashContext, StaticTableRow>();
            await DeleteAccountAt<DashContext, StaticFee>();

            await DeleteAccountAt<DashContext, SelectOneFlat>();
            await DeleteAccountAt<DashContext, PercentOfThreshold>();
            await DeleteAccountAt<DashContext, BasicThreshold>();
            await DeleteAccountAt<DashContext, TwoNestedCategory>();
            await DeleteAccountAt<DashContext, CategoryNestedThreshold>();
            await DeleteAccountAt<DashContext, Logo>();
            await DeleteAccountAt<DashContext, StaticTablesMeta>();
            await DeleteAccountAt<DashContext, Area>();
            await DeleteAccountAt<DashContext, AttachmentLinkRecord>();

            await DeleteAccountAt<AccountsContext, Account>();
            await DeleteAccountAt<AccountsContext, Session>();
            await DeleteAccountAt<AccountsContext, EmailVerification>();
            await DeleteAccountAt<AccountsContext, Subscription>();
            await DeleteAccountAt<AccountsContext, StripeWebhookReceivedRecord>();


            await DeleteAccountAt<ConvoContext, ConversationHistory>();
            await DeleteAccountAt<ConvoContext, ConversationRecord>();

            if (Counter != allEntities.Count - 1) throw new DomainException($"Failed to delete all data - contact palavyr. hit {Counter} of {allEntities.Count}");
            await contextProvider.DangerousCommitAllContexts();

            await DeleteStripeCustomer(stripeCustomerId);
        }

        public async Task DeleteStripeCustomer(string stripeCustomerId)
        {
            var customerService = lifetimeScope.GetService<IStripeCustomerService>();
            await customerService.DeleteSingleStripeTestCustomer(stripeCustomerId);
        }

        internal virtual List<TEntity> FilterByCurrentAccount<TEntity>(List<TEntity> unfiltered)
        {
            var entities = new List<TEntity>();
            try
            {
                var selected = unfiltered.Select(x => (IHaveAccountId)x);
                foreach (var select in selected)
                {
                    if (select.AccountId == AccountId)
                    {
                        entities.Add((TEntity)select);
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
    }
}