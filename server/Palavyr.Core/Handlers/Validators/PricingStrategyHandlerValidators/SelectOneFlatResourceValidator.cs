using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Shouldly;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class SelectOneFlatResourceValidator : AbstractValidator<List<SelectOneFlatRowResource>>
    {
        public SelectOneFlatResourceValidator()
        {
            RuleForEach(c => c)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Option).ShouldNotBeNull().NotEmpty();
                        r.RuleFor(x => x.Range).NotEmpty().NotNull();
                        r.RuleFor(x => x.AccountId).NotEmpty().NotNull();
                        r.RuleFor(x => x.AreaIdentifier).NotEmpty().NotNull();
                        r.RuleFor(x => x.RowOrder).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotNull().NotEmpty();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotEmpty().When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });
            RuleFor(c => c).Must(CategoriesMustBeUnique).WithMessage("Categories must be unique.");
            RuleFor(c => c).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");
        }

        private bool HaveCorrectlyOrderedRows(List<SelectOneFlatRowResource> arg)
        {
            var orders = arg.Select(x => x.RowOrder).ToList();
            return Enumerable.SequenceEqual(orders, orders.OrderBy(x => x));
        }

        private bool CategoriesMustBeUnique(List<SelectOneFlatRowResource> arg)
        {
            return arg.Count == arg.Select(x => x.Option).Distinct().Count();
        }
    }
}