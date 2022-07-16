using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class PercentOfThresholdResourceValidator : AbstractValidator<PricingStrategyTableDataResource<PercentOfThresholdResource>>
    {
        public PercentOfThresholdResourceValidator()
        {
            RuleForEach(cr => cr.TableRows)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.Threshold).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.AccountId).NotEmpty();
                        r.RuleFor(x => x.IntentId).NotEmpty();
                        r.RuleFor(x => x.ItemName).NotEmpty();
                        r.RuleFor(x => x.RowId).NotEmpty();
                        r.RuleFor(x => x.TableId).NotNull();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TriggerFallback).NotNull();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue).When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                        r.RuleFor(x => x.Modifier).NotNull();
                    });
            RuleFor(x => x.TableRows).Must(MustHaveDistinctThresholds).WithMessage("Thresholds must all be unique values");
            RuleFor(x => x.TableRows).Must(HaveUniqueItemOrders).WithMessage("Item orders must be unique");
            RuleFor(x => x.TableRows).Must(HaveUniqueRowOrders).WithMessage("Row orders must be unique");
            RuleFor(x => x.TableRows).Must(HaveCorrectlyOrderedItems).WithMessage("Items must be correctly ordered");
            RuleFor(x => x.TableRows).Must(HaveCorrectlyOrderedRows).WithMessage("Rows must be correctly Ordered");
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