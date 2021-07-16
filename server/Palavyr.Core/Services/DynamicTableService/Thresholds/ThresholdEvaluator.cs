using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Services.DynamicTableService.Thresholds
{
    public interface IThresholdEvaluator
    {
        IOrderableThreshold Evaluate(double responseValue, IEnumerable<IOrderableThreshold> orderedThresholds);
        bool EvaluateForFallback(double responseValue, IEnumerable<IOrderableThreshold> orderedThresholds);
    }

    public class ThresholdEvaluator : IThresholdEvaluator
    {
        private List<IOrderableThreshold> OrderThresholdsByDescending(IEnumerable<IOrderableThreshold> orderedThresholds)
        {
            return orderedThresholds.OrderByDescending(x => x.Threshold).ToList(); // defensive. 0 cost if already sorted.
        }

        public IOrderableThreshold Evaluate(double responseValue, IEnumerable<IOrderableThreshold> orderedThresholds)
        {
            
            // the convo node then is responsible for adding the extra logic for min and max.

            var thresholdsOrderedByDescending = OrderThresholdsByDescending(orderedThresholds);
            if (!thresholdsOrderedByDescending.Any()) throw new Exception("Cannot evaluate when there are no thresholds provided");

            // the chat bot has already dealt with cases where we are below the first threshold (too complicated) and above the max threshold (too complicated).
            // :thinking: how do we safeguard a case where we get a below or above. This sounds like an exception scenario.
            IOrderableThreshold threshold = null;
            for (var i = 0; i < thresholdsOrderedByDescending.Count; i++)
            {
                threshold = thresholdsOrderedByDescending[i];
                if (responseValue >= threshold.Threshold) // I really want this to be readable.
                {
                    break;
                }
            }

            // the null check is for the compiler...
            if (threshold != null && threshold.TriggerFallback) // this should always be false
            {
                throw new Exception("When Triggering the max threshold, the widget should provide a Too Complicated scenario");
            }

            return threshold;
        }

        public bool EvaluateForFallback(double responseValue, IEnumerable<IOrderableThreshold> thresholds)
        {
            var orderedThresholds = OrderThresholdsByDescending(thresholds);
            if (responseValue < orderedThresholds.Last().Threshold)
            {
                return true;
            }

            if (responseValue > orderedThresholds.First().Threshold && orderedThresholds.First().TriggerFallback)
            {
                return true;
            }

            return false;
        }
    }
}