﻿using System.Threading.Tasks;
using Palavyr.Core.Data;

namespace Palavyr.Core.Stores
{
    public interface IUnitOfWorkContextProvider
    {
        AccountsContext AccountsContext();
        ConvoContext ConvoContext();
        DashContext ConfigurationContext();
        Task CloseUnitOfWork();
        Task DisposeContexts();
        Task DangerousCommitAllContexts();

        Task OpenUnitOfWorkAsync();
        void OpenUnitOfWork();
    }
}