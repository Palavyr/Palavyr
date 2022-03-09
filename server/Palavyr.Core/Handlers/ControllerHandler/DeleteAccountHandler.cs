﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteAccountHandler : INotificationHandler<DeleteAccountNotification>
    {
        private readonly IDangerousAccountDeleter dangerousAccountDeleter;
        private readonly ILogger<DeleteAccountHandler> logger;
        private readonly IConfigurationEntityStore<Account> accountStore;

        public DeleteAccountHandler(
            IDangerousAccountDeleter dangerousAccountDeleter,
            ILogger<DeleteAccountHandler> logger,
            IConfigurationEntityStore<Account> accountStore
        )
        {
            this.dangerousAccountDeleter = dangerousAccountDeleter;
            this.logger = logger;
            this.accountStore = accountStore;
        }

        public async Task Handle(DeleteAccountNotification notification, CancellationToken _)
        {
            logger.LogInformation($"Deleting details for account: {accountStore.AccountId}");
            await dangerousAccountDeleter.DeleteAllThings();
        }
    }

    public class DeleteAccountNotification : INotification
    {
    }
}