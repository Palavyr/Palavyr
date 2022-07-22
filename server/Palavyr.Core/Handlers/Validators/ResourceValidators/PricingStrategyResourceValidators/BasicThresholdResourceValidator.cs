using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.ResourceValidators.PricingStrategyResourceValidators
{
    public class BasicThresholdResourceValidator : AbstractValidator<PricingStrategyTableDataResource<SimpleThresholdResource>>
    {
        public BasicThresholdResourceValidator()
        {
            RuleForEach(c => c.TableRows)
                .ChildRules(
                    r =>
                    {
                        r.RuleFor(x => x.Range).NotNull();
                        r.RuleFor(x => x.Threshold).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.IntentId).NotEmpty();
                        r.RuleFor(x => x.ItemName).NotEmpty();
                        r.RuleFor(x => x.RowId).NotEmpty();
                        r.RuleFor(x => x.TableId).NotEmpty();
                        r.RuleFor(x => x.RowOrder).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.TriggerFallback).NotNull();
                        r.RuleFor(x => x.ValueMin).NotNull().GreaterThanOrEqualTo(0);
                        r.RuleFor(x => x.ValueMax).NotNull().LessThanOrEqualTo(int.MaxValue).When(x => x.Range);
                        r.RuleFor(x => x.ValueMax).NotNull().GreaterThanOrEqualTo(x => x.ValueMin).When(x => x.Range);
                    });

            RuleFor(x => x.TableRows).Must(MustHaveDistinctThresholds).WithMessage("Thresholds must all be unique values.");
            RuleFor(x => x.TableRows).Must(HaveDistinctRowOrders).WithMessage("Row orders must be unique");
            RuleFor(x => x.TableRows).Must(BeOrderedCorrectly).WithMessage("Rows must be ordered correctly");
        }

        private bool BeOrderedCorrectly(List<SimpleThresholdResource> arg)
        {
            var tableIdGroups = arg.GroupBy(x => x.TableId);
            var isValid = true;
            foreach (var group in tableIdGroups)
            {
                var ordered = group
                    .OrderBy(x => x.RowId)
                    .Select(x => x.ValueMin)
                    .ToArray();
                if (!Enumerable.SequenceEqual(ordered, ordered.OrderBy(x => x)))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool HaveDistinctRowOrders(List<SimpleThresholdResource> arg)
        {
            return arg.Select(x => x.RowOrder).Count() == arg.Select(x => x.RowOrder).Distinct().Count();
        }

        private bool MustHaveDistinctThresholds(List<SimpleThresholdResource> arg)
        {
            return arg.Select(x => x.Threshold).Distinct().ToList().Count == arg.Count;
        }
    }
}