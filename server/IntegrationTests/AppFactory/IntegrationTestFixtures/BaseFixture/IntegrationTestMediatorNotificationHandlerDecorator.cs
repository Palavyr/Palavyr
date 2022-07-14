using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Stores;

namespace IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public class IntegrationTestMediatorNotificationHandlerDecorator<TEvent> : INotificationHandler<TEvent> where TEvent : INotification
    {
        private readonly INotificationHandler<TEvent> inner;
        private readonly IUnitOfWorkContextProvider unitOfWorkContextProvider;

        public IntegrationTestMediatorNotificationHandlerDecorator(INotificationHandler<TEvent> inner, IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            this.inner = inner;
            this.unitOfWorkContextProvider = unitOfWorkContextProvider;
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            await inner.Handle(notification, cancellationToken);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }
    }
}