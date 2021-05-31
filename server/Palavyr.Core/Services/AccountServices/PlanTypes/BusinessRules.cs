using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public interface IBusinessRules
    {
        Task<PlanTypeMeta> GetPlanTypeMeta(string accountId, CancellationToken cancellationToken);
        Task<int> GetAllowedAttachments(string accountId, CancellationToken cancellationToken);
        Task<int> GetAllowedStaticTables(string accountId, CancellationToken cancellationToken);
        Task<int> GetAllowedDynamicTables(string accountId, CancellationToken cancellationToken);
        Task<int> GetAllowedAreas(string accountId, CancellationToken cancellationToken);
        Task<bool> GetAllowedImageUpload(string accountId, CancellationToken cancellationToken);
        Task<bool> GetAllowedEmailNotifications(string accountId, CancellationToken cancellationToken);
        Task<bool> GetAllowedInlineEmailEditor(string accountId, CancellationToken cancellationToken);
        Task<bool> GetAllowedSmsNotification(string accountId, CancellationToken cancellationToken);
    }

    public class BusinessRules : IBusinessRules
    {
        private readonly IPlanTypeRetriever planTypeRetriever;

        public BusinessRules(IPlanTypeRetriever planTypeRetriever)
        {
            this.planTypeRetriever = planTypeRetriever;
        }

        public async Task<PlanTypeMeta> GetPlanTypeMeta(string accountId, CancellationToken cancellationToken)
        {
            var planType = await planTypeRetriever.GetCurrentPlanType(accountId, cancellationToken);
            if (planType == Account.PlanTypeEnum.Free.ToString())
            {
                return new FreePlanTypeMeta();
            }
            else if (planType == Account.PlanTypeEnum.Lyte.ToString())
            {
                return new LytePlanTypeMeta();
            }
            else if (planType == Account.PlanTypeEnum.Premium.ToString())
            {
                return new PremiumPlanTypeMeta();
            }
            else if (planType == Account.PlanTypeEnum.Pro.ToString())
            {
                return new ProPlanTypeMeta();
            }
            else
            {
                throw new DomainException($"Failed to find the requested plan type: {planType}");
            }
        }


        public async Task<int> GetAllowedAttachments(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedAttachments;
        }

        public async Task<int> GetAllowedStaticTables(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedStaticTables;
        }

        public async Task<int> GetAllowedDynamicTables(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedDynamicTables;
        }

        public async Task<int> GetAllowedAreas(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedAreas;
        }

        public async Task<bool> GetAllowedImageUpload(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedImageUpload;
        }

        public async Task<bool> GetAllowedEmailNotifications(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedEmailNotifications;
        }

        public async Task<bool> GetAllowedInlineEmailEditor(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedInlineEmailEditor;
        }

        public async Task<bool> GetAllowedSmsNotification(string accountId, CancellationToken cancellationToken)
        {
            var currentPlan = await GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan.AllowedSmsNotifications;
        }
    }
}