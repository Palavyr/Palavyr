using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Stores;

namespace Component.AppFactory.ComponentTestBase.BaseFixture
{
    public class IntegrationTestMediatorRequestHandlerDecorator<TEvent, TResponse> : IRequestHandler<TEvent, TResponse> where TEvent : IRequest<TResponse>
    {
        private readonly IRequestHandler<TEvent, TResponse> inner;
        private readonly IUnitOfWorkContextProvider unitOfWorkContextProvider;

        public IntegrationTestMediatorRequestHandlerDecorator(IRequestHandler<TEvent, TResponse> inner, IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            this.inner = inner;
            this.unitOfWorkContextProvider = unitOfWorkContextProvider;
        }

        public async Task<TResponse> Handle(TEvent request, CancellationToken cancellationToken)
        {
            var response = await inner.Handle(request, cancellationToken);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return response;
        }
    }
}