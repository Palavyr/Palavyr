using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.ResourceValidators.PricingStrategyResourceValidators
{
    public class CategoryNestedThresholdValidator : AbstractValidator<PricingStrategyTableDataResource<CategoryNestedThresholdResource>>
    {
        public CategoryNestedThresholdValidator()
        {
            RuleForEach(c => c.TableRows)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.Threshold).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.IntentId).NotEmpty();
                        r.RuleFor(x => x.ItemId).NotEmpty();
                        r.RuleFor(x => x.ItemName).NotEmpty();
                        r.RuleFor(x => x.RowId).NotEmpty();
                        r.RuleFor(x => x.ItemOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TriggerFallback).NotNull();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue).When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });
            RuleFor(c => c.TableRows).Must(HaveDistinctThresholds).WithMessage("Thresholds must all be unique values");
            RuleFor(c => c.TableRows).Must(HaveDistinctItemOrders).WithMessage("Item orders must be distinct");
            RuleFor(c => c.TableRows).Must(HaveDistinctRowOrders).WithMessage("Row orders must be distinct");
            RuleFor(c => c.TableRows).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(c => c.TableRows).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");
            RuleFor(c => c.TableRows).Must(HaveItemWithMatchingOrderIds).WithMessage("Categories for a single item must have matching order Ids");
        }


        private bool HaveCorrectlyOrderedRows(List<CategoryNestedThresholdResource> arg)
        {
            var tableIdGroups = arg.GroupBy(x => x.ItemId);
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

        private bool HaveCorrectlyOrderedItems(List<CategoryNestedThresholdResource> arg)
        {
            var tableOrders = arg.Select(x => x.ItemOrder).Distinct().ToList();
            return Enumerable.SequenceEqual(tableOrders, tableOrders.OrderBy(x => x));
        }

        private bool HaveDistinctRowOrders(List<CategoryNestedThresholdResource> arg)
        {
            foreach (var group in arg.GroupBy(x => x.ItemId))
            {
                var orders = group.Select(x => x.RowOrder).ToList();
                if (orders.Count != orders.Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveItemWithMatchingOrderIds(List<CategoryNestedThresholdResource> arg)
        {
            foreach (var group in arg.GroupBy(x => x.ItemId))
            {
                var o = group.Select(x => x.ItemOrder).ToList();
                if (o.Distinct().Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveDistinctItemOrders(List<CategoryNestedThresholdResource> arg)
        {
            var orders = new List<int>();
            foreach (var group in arg.GroupBy(x => x.ItemId))
            {
                var o = group.Select(x => x.ItemOrder).ToList();
                orders.Add(o.Distinct().Single());
            }

            return orders.Count == orders.Distinct().Count();
        }

        private bool HaveDistinctThresholds(List<CategoryNestedThresholdResource> arg)
        {
            var tableIdGroups = arg.GroupBy(x => x.ItemId);
            var isValid = true;
            foreach (var group in tableIdGroups)
            {
                var originalLength = group.ToList().Count;
                var distinct = group.Select(x => x.Threshold).Distinct().ToArray();
                if (distinct.Length < originalLength)
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}