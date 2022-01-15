using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Palavyr.Core.Handlers
{
    public class TemplateHandler : IRequestHandler<TemplateRequest, TemplateResponse>
    {

        public TemplateHandler()
        {
        }

        public async Task<TemplateResponse> Handle(TemplateRequest request, CancellationToken cancellationToken)
        {

        }
    }

    public class TemplateResponse
    {
        public TemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class TemplateRequest : IRequest<TemplateResponse>
    {
        public string CompanyName { get; set; }
    }
}
