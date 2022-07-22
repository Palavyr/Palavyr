using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class CategorySelectResourceValidator : AbstractValidator<PricingStrategyTableDataResource<CategorySelectTableRowResource>>
    {
        public CategorySelectResourceValidator()
        {
            RuleFor(c => c.TableTag).NotEmpty().WithMessage("A table tag must be provided");
            
            RuleForEach(c => c.TableRows)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Category).NotEmpty();
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.IntentId).NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotEmpty();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotNull().When(x => x.Range == true);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });
            RuleFor(c => c.TableRows).Must(CategoriesMustBeUnique).WithMessage("Options must be unique.");
            RuleFor(c => c.TableRows).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");
        }

        // Do we need this...
        private bool HaveCorrectlyOrderedRows(List<CategorySelectTableRowResource> arg)
        {
            var orders = arg.Select(x => x.RowOrder).ToList();
            return Enumerable.SequenceEqual(orders, orders.OrderBy(x => x));
        }

        private bool CategoriesMustBeUnique(List<CategorySelectTableRowResource> arg)
        {
            return arg.Count == arg.Select(x => x.Category).Distinct().Count();
        }
    }
}