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
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Contracts;
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
            AppDataContexts appDataContexts,
            AccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
            : base(
                serviceProvider,
                unitOfWorkContextProvider, fileAssetDeleter, appDataContexts, accountIdTransport, cancellationTokenTransport)
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
        private readonly AppDataContexts appDataContexts;
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
            AppDataContexts appDataContexts,
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport)
        {
            this.lifetimeScope = lifetimeScope;
            this.contextProvider = contextProvider;
            this.fileAssetDeleter = fileAssetDeleter;
            this.appDataContexts = appDataContexts;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;

            allEntities.AddRange(appDataContexts.Model.GetEntityTypes().ToList());
        }

        public async Task DeleteAllThings()
        {
            await DeleteFileAssets();
            await DeleteAccountEntities();
        }

        internal virtual async Task DeleteFileAssets()
        {
            var fileAssetIds = await appDataContexts
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

            var account = await accountStore.GetAccount();
            var stripeCustomerId = account.StripeCustomerId;

            // ORDER MATTERS HERE SINCE WE LINK TABLES IN THE ORM AND WE DONT USE REFLECTION TO AUTO-INCLUDE ALL CHILD ENTITIES from the aggregate root
            await DeleteAccountAt<AppDataContexts, ConversationNode>();
            await DeleteAccountAt<AppDataContexts, PricingStrategyTableMeta>();
            await DeleteAccountAt<AppDataContexts, FileAsset>();
            await DeleteAccountAt<AppDataContexts, WidgetPreference>();

            await DeleteAccountAt<AppDataContexts, StaticTableRow>();
            await DeleteAccountAt<AppDataContexts, StaticFee>();

            await DeleteAccountAt<AppDataContexts, CategorySelectTableRow>();
            await DeleteAccountAt<AppDataContexts, PercentOfThresholdTableRow>();
            await DeleteAccountAt<AppDataContexts, SimpleThresholdTableRow>();
            await DeleteAccountAt<AppDataContexts, TwoNestedSelectTableRow>();
            await DeleteAccountAt<AppDataContexts, CategoryNestedThresholdTableRow>();
            await DeleteAccountAt<AppDataContexts, Logo>();
            await DeleteAccountAt<AppDataContexts, StaticTablesMeta>();
            await DeleteAccountAt<AppDataContexts, Intent>();
            await DeleteAccountAt<AppDataContexts, AttachmentLinkRecord>();

            await DeleteAccountAt<AppDataContexts, Account>();
            await DeleteAccountAt<AppDataContexts, Session>();
            await DeleteAccountAt<AppDataContexts, EmailVerification>();
            await DeleteAccountAt<AppDataContexts, Subscription>();
            await DeleteAccountAt<AppDataContexts, StripeWebhookReceivedRecord>();

            await DeleteAccountAt<AppDataContexts, ConversationHistoryRow>();
            await DeleteAccountAt<AppDataContexts, ConversationHistoryMeta>();

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
                var selected = unfiltered.Select(x => x as IHaveAccountId).Where(x => x != null);
                foreach (var select in selected)
                {
                    if (select?.AccountId == AccountId)
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