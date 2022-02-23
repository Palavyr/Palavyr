using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteCompanyLogoHandler : INotificationHandler<DeleteCompanyLogoRequest>
    {
        private readonly ILogoDeleter logoDeleter;

        public DeleteCompanyLogoHandler(ILogoDeleter logoDeleter)
        {
            this.logoDeleter = logoDeleter;
        }

        public async Task Handle(DeleteCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            await logoDeleter.DeleteLogo();
        }
    }

    public class DeleteCompanyLogoResponse
    {
        public string Response { get; set; }
        public DeleteCompanyLogoResponse(string response) => Response = response;
    }

    public class DeleteCompanyLogoRequest : INotification
    {
    }
}