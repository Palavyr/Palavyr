using MediatR;

namespace Palavyr.Core.Mediator
{
    public class PalavyrMediator : MediatR.Mediator
    {
        public PalavyrMediator(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }
    }
}