using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class TwoNestedCategoryResourceValidator : AbstractValidator<List<TwoNestedCategoryResource>>
    {
        public TwoNestedCategoryResourceValidator()
        {
            RuleFor(c => c).Must(HaveUniqueOuterCategories).WithMessage("Outer categories must be unique");
            RuleFor(c => c).Must(HaveUniqueInnerCategories).WithMessage("Inner categories must be unique");
            RuleFor(c => c).Must(HaveUniqueRowOrders).WithMessage("Row orders must be unique");
            RuleFor(c => c).Must(HaveUniqueItemOrders).WithMessage("Item orders must be unique");
            RuleFor(c => c).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(c => c).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");

            RuleForEach(c => c)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.AccountId).NotNull().NotEmpty();
                        r.RuleFor(x => x.AreaIdentifier).NotNull().NotEmpty();
                        r.RuleFor(x => x.ItemId).NotNull().NotEmpty();
                        r.RuleFor(x => x.ItemName).NotNull().NotEmpty();
                        r.RuleFor(x => x.ItemOrder).GreaterThanOrEqualTo(0).NotNull();
                        r.RuleFor(x => x.RowId).NotNull().NotEmpty();
                        r.RuleFor(x => x.RowOrder).GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotNull().NotEmpty();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotEmpty().When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                        r.RuleFor(x => x.InnerItemName).NotNull().NotEmpty();
                    });
        }

        private bool HaveUniqueItemOrders(List<TwoNestedCategoryResource> arg)
        {
            var itemOrders = arg.Select(x => x.ItemOrder).ToList();
            return itemOrders.Count() == itemOrders.Distinct().Count();
        }

        private bool HaveUniqueRowOrders(List<TwoNestedCategoryResource> arg)
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

        private bool HaveUniqueOuterCategories(List<TwoNestedCategoryResource> arg)
        {
            var itemIds = arg.Select(x => x.ItemId).Distinct().ToList();
            var itemNames = arg.Select(x => x.ItemName).Distinct().ToList();
            return itemNames.Count() == itemIds.Count();
        }

        private bool HaveUniqueInnerCategories(List<TwoNestedCategoryResource> arg)
        {
            var itemId = arg.Select(x => x.ItemId).Distinct().ToList().First();
            var innerItemNames = arg.Where(x => x.ItemId == itemId).Select(x => x.InnerItemName).ToList();

            return innerItemNames.Count() == innerItemNames.Distinct().Count();
        }

        private bool HaveCorrectlyOrderedRows(List<TwoNestedCategoryResource> arg)
        {
            var tableIdGroups = arg.GroupBy(x => x.TableId);
            foreach (var group in tableIdGroups)
            {
                var rowOrders = group.Select(x => x.RowOrder).ToList();
                if (!Enumerable.SequenceEqual(rowOrders, rowOrders.OrderBy(x => x)))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveCorrectlyOrderedItems(List<TwoNestedCategoryResource> arg)
        {
            var tableOrders = arg.Select(x => x.ItemOrder).Distinct().ToList();
            return Enumerable.SequenceEqual(tableOrders, tableOrders.OrderBy(x => x));
        }
    }
}