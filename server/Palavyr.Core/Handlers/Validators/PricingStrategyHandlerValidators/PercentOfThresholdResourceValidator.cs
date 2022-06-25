﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Shouldly;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class PercentOfThresholdResourceValidator : AbstractValidator<List<PercentOfThresholdResource>>
    {
        public PercentOfThresholdResourceValidator()
        {
            RuleForEach(cr => cr)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotEmpty().NotNull();
                        r.RuleFor(x => x.Threshold).NotEmpty().NotNull();
                        r.RuleFor(x => x.AccountId).NotEmpty().NotNull();
                        r.RuleFor(x => x.AreaIdentifier).NotEmpty().NotNull();
                        r.RuleFor(x => x.ItemName).NotEmpty().NotNull();
                        r.RuleFor(x => x.RowId).NotEmpty().NotNull();
                        r.RuleFor(x => x.TableId).NotNull().NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TriggerFallback).NotNull();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue);
                        r.RuleFor(x => x.ValueMax).NotEmpty().When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                        r.RuleFor(x => x.Modifier).ShouldNotBeNull();
                    });
            RuleFor(x => x).Must(MustHaveDistinctThresholds).WithMessage("Thresholds must all be unique values.");
            RuleFor(x => x).Must(HaveUniqueItemOrders).WithMessage("Item orders must be unique");
            RuleFor(x => x).Must(HaveUniqueRowOrders).WithMessage("Row orders must be unique");
            RuleFor(x => x).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(x => x).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly Ordered");
        }

        private bool HaveUniqueRowOrders(List<PercentOfThresholdResource> arg)
        {
            foreach (var group in arg.GroupBy(x => x.ItemId))
            {
                if (group.Select(x => x.RowOrder).Count() != group.Select(x => x.RowOrder).Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveUniqueItemOrders(List<PercentOfThresholdResource> arg)
        {
            return arg.Select(x => x.ItemOrder).Count() == arg.Select(x => x.ItemOrder).Distinct().Count();
        }

        private bool MustHaveDistinctThresholds(List<PercentOfThresholdResource> arg)
        {
            var groups = arg.GroupBy(x => x.TableId);
            foreach (var group in groups)
            {
                if (group.Count() != group.Select(x => x.Threshold).Distinct().Count())
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveCorrectlyOrderedRows(List<PercentOfThresholdResource> arg)
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

        private bool HaveCorrectlyOrderedItems(List<PercentOfThresholdResource> arg)
        {
            var tableOrders = arg.Select(x => x.ItemOrder).Distinct().ToList();
            return Enumerable.SequenceEqual(tableOrders, tableOrders.OrderBy(x => x));
        }
    }
}