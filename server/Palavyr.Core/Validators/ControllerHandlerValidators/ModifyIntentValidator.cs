using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Shouldly;

namespace Palavyr.Core.Validators.ControllerHandlerValidators
{
    public class ModifyIntentValidator : IRequestValidator<ModifyAreaEmailTemplateRequest, ModifyAreaEmailTemplateResponse>
    {
        public async Task Validate(ModifyAreaEmailTemplateRequest request)
        {
            request.EmailTemplate.ShouldNotBeNullOrEmpty("The Email Template must not be empty.");
            request.IntentId.ShouldNotBeNullOrEmpty("The Intent Id must be supplied.");
        }
    }
}