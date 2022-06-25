using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class SelectOneFlatResourceValidator : AbstractValidator<List<SelectOneFlatResource>>
    {
        public SelectOneFlatResourceValidator()
        {
            RuleForEach(c => c)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Option).NotEmpty();
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.AccountId).NotEmpty();
                        r.RuleFor(x => x.AreaIdentifier).NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotEmpty();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotNull().When(x => x.Range == true);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });
            RuleFor(c => c).Must(CategoriesMustBeUnique).WithMessage("Options must be unique.");
            RuleFor(c => c).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");
        }
        
        // Do we need this...
        private bool HaveCorrectlyOrderedRows(List<SelectOneFlatResource> arg)
        {
            var orders = arg.Select(x => x.RowOrder).ToList();
            return Enumerable.SequenceEqual(orders, orders.OrderBy(x => x));
        }

        private bool CategoriesMustBeUnique(List<SelectOneFlatResource> arg)
        {
            return arg.Count == arg.Select(x => x.Option).Distinct().Count();
        }
    }
}