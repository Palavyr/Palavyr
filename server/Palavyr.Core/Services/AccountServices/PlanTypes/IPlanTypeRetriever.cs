using System.Threading.Tasks;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public interface IPlanTypeRetriever
    {
        Task<string> GetCurrentPlanType();
    }
}