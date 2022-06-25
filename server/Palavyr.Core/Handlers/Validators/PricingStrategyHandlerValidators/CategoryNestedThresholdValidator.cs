﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class CategoryNestedThresholdValidator : AbstractValidator<List<CategoryNestedThresholdResource>>
    {
        public CategoryNestedThresholdValidator()
        {
            RuleForEach(c => c)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotEmpty().NotNull();
                        r.RuleFor(x => x.Threshold).NotEmpty().NotNull();
                        r.RuleFor(x => x.AccountId).NotEmpty().NotNull();
                        r.RuleFor(x => x.AreaIdentifier).NotEmpty().NotNull();
                        r.RuleFor(x => x.ItemId).NotEmpty().NotNull();
                        r.RuleFor(x => x.ItemName).NotEmpty().NotNull();
                        r.RuleFor(x => x.RowId).NotEmpty().NotNull();
                        r.RuleFor(x => x.ItemOrder).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TableId).NotNull().NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TriggerFallback).NotNull();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotEmpty().When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });
            RuleFor(c => c).Must(HaveDistinctThresholds).WithMessage("Thresholds must all be unique values.");
            RuleFor(c => c).Must(HaveDistinctItemOrders).WithMessage("Item orders must be distinct");
            RuleFor(c => c).Must(HaveDistinctRowOrders).WithMessage("Row orders must be distinct");
            RuleFor(c => c).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(c => c).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly ordered");
        }


        private bool HaveCorrectlyOrderedRows(List<CategoryNestedThresholdResource> arg)
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

        private bool HaveCorrectlyOrderedItems(List<CategoryNestedThresholdResource> arg)
        {
            var tableOrders = arg.Select(x => x.ItemOrder).Distinct().ToList();
            return Enumerable.SequenceEqual(tableOrders, tableOrders.OrderBy(x => x));
        }

        private bool HaveDistinctRowOrders(List<CategoryNestedThresholdResource> arg)
        {
            foreach (var group in arg.GroupBy(x => x.TableId))
            {
                var orders = group.Select(x => x.RowOrder).ToList();
                if (orders.Count != orders.Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveDistinctItemOrders(List<CategoryNestedThresholdResource> arg)
        {
            var orders = arg.Select(x => x.ItemOrder).ToList();
            return orders.Count == orders.Distinct().Count();
        }

        private bool HaveDistinctThresholds(List<CategoryNestedThresholdResource> arg)
        {
            var tableIdGroups = arg.GroupBy(x => x.TableId);
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