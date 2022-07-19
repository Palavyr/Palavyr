using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class IntentResourceMapper : IMapToNew<Intent, IntentResource>
    {
        public async Task<IntentResource> Map(Intent @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new IntentResource
            {
                IntentId = @from.IntentId,
                IntentName = @from.IntentName,
                Prologue = @from.Prologue,
                Epilogue = @from.Epilogue,
                EmailTemplate = @from.EmailTemplate,
                Subject = @from.Subject,
                FallbackEmailTemplate = @from.FallbackEmailTemplate,
                FallbackSubject = @from.FallbackSubject,
                ConversationNodes = @from.ConversationNodes,
                StaticTablesMetas = @from.StaticTablesMetas,
                IsEnabled = @from.IsEnabled,
                PricingStrategyTableMetas = @from.PricingStrategyTableMetas,
                IntentSpecificEmail = @from.IntentSpecificEmail,
                EmailIsVerified = @from.EmailIsVerified,
                SendPdfResponse = @from.SendPdfResponse,
                IncludePricingStrategyTableTotals = @from.IncludePricingStrategyTableTotals
            };
        }
    }
}