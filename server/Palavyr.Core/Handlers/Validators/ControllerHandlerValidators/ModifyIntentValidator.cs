using FluentValidation;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.Core.Handlers.Validators.ControllerHandlerValidators
{
    public class ModifyIntentValidator : AbstractValidator<ModifyIntentEmailTemplateRequest>
    {
        public ModifyIntentValidator()
        {
            RuleFor(c => c.EmailTemplate)
                .NotEmpty()
                .NotNull()
                .WithMessage("The Email Template must not be empty");
            RuleFor(c => c.IntentId)
                .NotEmpty()
                .NotNull()
                .WithMessage("The intent id must be provided.");
        }
    }
}