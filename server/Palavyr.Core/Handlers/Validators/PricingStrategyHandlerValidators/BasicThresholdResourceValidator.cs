using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    public class BasicThresholdResourceValidator : AbstractValidator<List<BasicThresholdResource>>
    {
        public BasicThresholdResourceValidator()
        {
            RuleForEach(c => c)
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
                    });

            RuleFor(x => x).Must(MustHaveDistinctThresholds).WithMessage("Thresholds must all be unique values.");
            RuleFor(x => x).Must(HaveDistinctRowOrders).WithMessage("Row orders must be unique");
            RuleFor(x => x).Must(BeOrderedCorrectly).WithMessage("Rows must be ordered correctly");
        }

        private bool BeOrderedCorrectly(List<BasicThresholdResource> arg)
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

        private bool HaveDistinctRowOrders(List<BasicThresholdResource> arg)
        {
            return arg.Select(x => x.RowOrder).Count() == arg.Select(x => x.RowOrder).Distinct().Count();
        }

        private bool MustHaveDistinctThresholds(List<BasicThresholdResource> arg)
        {
            return arg.Select(x => x.Threshold).Distinct().ToList().Count == arg.Count;
        }
    }
}