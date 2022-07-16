using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public interface IBusinessRules
    {
        Task<PlanTypeMetaBase> GetPlanTypeMeta();
        Task<int> GetAllowedAttachments();
        Task<int> GetAllowedStaticTables();
        Task<int> GetAllowedDynamicTables();
        Task<int> GetAllowedIntents();
        Task<bool> GetAllowedImageUpload();
        Task<bool> GetAllowedEmailNotifications();
        Task<bool> GetAllowedInlineEmailEditor();
        Task<bool> GetAllowedSmsNotification();
    }

    public class BusinessRules : IBusinessRules
    {
        private readonly IPlanTypeRetriever planTypeRetriever;
        private readonly IAccountIdTransport accountIdTransport;

        public BusinessRules(IPlanTypeRetriever planTypeRetriever, IAccountIdTransport accountIdTransport)
        {
            this.planTypeRetriever = planTypeRetriever;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<PlanTypeMetaBase> GetPlanTypeMeta()
        {
            var planType = await planTypeRetriever.GetCurrentPlanType();
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


        public async Task<int> GetAllowedAttachments()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedAttachments;
        }

        public async Task<int> GetAllowedStaticTables()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedStaticTables;
        }

        public async Task<int> GetAllowedDynamicTables()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedDynamicTables;
        }

        public async Task<int> GetAllowedIntents()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedIntents;
        }

        public async Task<bool> GetAllowedImageUpload()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedFileUpload;
        }

        public async Task<bool> GetAllowedEmailNotifications()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedEmailNotifications;
        }

        public async Task<bool> GetAllowedInlineEmailEditor()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedInlineEmailEditor;
        }

        public async Task<bool> GetAllowedSmsNotification()
        {
            var currentPlan = await GetPlanTypeMeta();
            return currentPlan.AllowedSmsNotifications;
        }
    }
}