using System;
using System.Linq;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Services.DynamicTableService.Thresholds
{
    public interface IThresholdEvaluator
    {
        IOrderableThreshold Evaluate(double responseValue, IOrderedEnumerable<IOrderableThreshold> orderedThresholds);
        bool EvaluateForFallback(double responseValue, IOrderedEnumerable<IOrderableThreshold> orderedThresholds);
    }

    public class ThresholdEvaluator : IThresholdEvaluator
    {
        public IOrderableThreshold Evaluate(double responseValue, IOrderedEnumerable<IOrderableThreshold> orderedThresholds)
        {
            // the convo node then is responsible for adding the extra logic for min and max.

            var reorderedThresholds = orderedThresholds.OrderBy(x => x.Threshold).ToList(); // defensive. 0 cost if already sorted.
            if (!reorderedThresholds.Any()) throw new Exception("Cannot evaluate when there are no thresholds provided");

            // the chat bot has already dealt with cases where we are below the first threshold (too complicated) and above the max threshold (too complicated).
            // :thinking: how do we safeguard a case where we get a below or above. This sounds like an exception scenario.
            IOrderableThreshold threshold = null;
            for (var i = 0; i < reorderedThresholds.Count; i++)
            {
                threshold = reorderedThresholds[i];
                if (responseValue <= threshold.Threshold) // I really want this to be readable.
                {
                    if (i == 0)
                    {
                        throw new Exception("We should never fail the first threshold at this point. The widget should catch this."); // I'll improve this when I can think of what to say here. Should probably be something handled by the error middleware.
                    }

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

        public bool EvaluateForFallback(double responseValue, IOrderedEnumerable<IOrderableThreshold> orderedThresholds)
        {
            var thresholds = orderedThresholds.ToList();
            if (responseValue < thresholds.First().Threshold)
            {
                return true;
            }

            if (responseValue > thresholds.Last().Threshold && thresholds.Last().TriggerFallback)
            {
                return true;
            }

            return false;

            // // the convo node then is responsible for adding the extra logic for min and max.
            //
            // var reorderedThresholds = orderedThresholds.OrderBy(x => x.Threshold).ToList(); // defensive. 0 cost if already sorted.
            // if (!reorderedThresholds.Any()) throw new Exception("Cannot evaluate when there are no thresholds provided");
            //
            // // the chat bot has already dealt with cases where we are below the first threshold (too complicated) and above the max threshold (too complicated).
            // // :thinking: how do we safeguard a case where we get a below or above. This sounds like an exception scenario.
            // IOrderableThreshold threshold = null;
            // for (var i = 0; i < reorderedThresholds.Count; i++)
            // {
            //     threshold = reorderedThresholds[i];
            //     if (responseValue <= threshold.Threshold) // I really want this to be readable.
            //     {
            //         if (i == 0)
            //         {
            //             return true;
            //         }
            //
            //         break;
            //     }
            // }
            //
            // // the null check is for the compiler...
            // if (threshold != null && threshold.TriggerFallback) // this should always be false
            // {
            //     return true;
            // }
            //
            // return false;
        }
    }
}