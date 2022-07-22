using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class TwoNestedCategoryResourceValidator : AbstractValidator<PricingStrategyTableDataResource<SelectWithNestedSelectResource>>
    {
        public TwoNestedCategoryResourceValidator()
        {
            RuleFor(c => c.TableRows).Must(HaveUniqueOuterCategories).WithMessage("Outer categories must be unique");
            RuleFor(c => c.TableRows).Must(HaveUniqueInnerCategories).WithMessage("Inner categories must be unique");
            RuleFor(c => c.TableRows).Must(HaveUniqueRowOrders).WithMessage("Row orders must be unique");
            RuleFor(c => c.TableRows).Must(HaveUniqueItemOrders).WithMessage("Item orders must be unique");
            RuleFor(c => c.TableRows).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(c => c.TableRows).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");

            RuleForEach(c => c.TableRows)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.IntentId).NotEmpty();
                        r.RuleFor(x => x.ItemId).NotEmpty();
                        r.RuleFor(x => x.ItemName).NotEmpty();
                        r.RuleFor(x => x.ItemOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.RowId).NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotEmpty();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue).When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).NotNull().GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                        r.RuleFor(x => x.InnerItemName).NotEmpty().WithMessage("Inner Category must not be empty");
                    });
        }

        private bool HaveUniqueItemOrders(List<SelectWithNestedSelectResource> arg)
        {
            
            
            var itemOrders = arg.Select(x => x.ItemOrder).ToList();
            return itemOrders.Count() == itemOrders.Distinct().Count();
        }

        private bool HaveUniqueRowOrders(List<SelectWithNestedSelectResource> arg)
        {
            var itemGroups = arg.GroupBy(x => x.ItemId);
            foreach (var group in itemGroups)
            {
                if (group.Select(x => x.RowOrder).Count() != group.Select(x => x.RowOrder).Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveUniqueOuterCategories(List<SelectWithNestedSelectResource> arg)
        {
            var itemIds = arg.Select(x => x.ItemId).Distinct().ToList();
            var itemNames = arg.Select(x => x.ItemName).Distinct().ToList();
            return itemNames.Count() == itemIds.Count();
        }

        private bool HaveUniqueInnerCategories(List<SelectWithNestedSelectResource> arg)
        {
            var itemId = arg.Select(x => x.ItemId).Distinct().ToList().First();
            var innerItemNames = arg.Where(x => x.ItemId == itemId).Select(x => x.InnerItemName).ToList();

            return innerItemNames.Count() == innerItemNames.Distinct().Count();
        }

        private bool HaveCorrectlyOrderedRows(List<SelectWithNestedSelectResource> arg)
        {
            var itemGroups = arg.GroupBy(x => x.ItemId);
            foreach (var group in itemGroups)
            {
                var rowOrders = group.Select(x => x.RowOrder).ToList();
                if (!Enumerable.SequenceEqual(rowOrders, rowOrders.OrderBy(x => x)))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveCorrectlyOrderedItems(List<SelectWithNestedSelectResource> arg)
        {
            var itemOrders = arg.Select(x => x.ItemOrder).Distinct().ToList();
            return Enumerable.SequenceEqual(itemOrders, itemOrders.OrderBy(x => x));
        }
    }
}