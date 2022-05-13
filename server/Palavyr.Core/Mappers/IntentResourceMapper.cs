using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class IntentResourceMapper : IMapToNew<Area, IntentResource>
    {
        public async Task<IntentResource> Map(Area @from)
        {
            await Task.CompletedTask;
            return new IntentResource
            {
                AreaIdentifier = @from.AreaIdentifier,
                AreaName = @from.AreaName,
                Prologue = @from.Prologue,
                Epilogue = @from.Epilogue,
                EmailTemplate = @from.EmailTemplate,
                Subject = @from.Subject,
                FallbackEmailTemplate = @from.FallbackEmailTemplate,
                FallbackSubject = @from.FallbackSubject,
                ConversationNodes = @from.ConversationNodes,
                StaticTablesMetas = @from.StaticTablesMetas,
                IsEnabled = @from.IsEnabled,
                AreaDisplayTitle = @from.AreaDisplayTitle,
                AccountId = @from.AccountId,
                DynamicTableMetas = @from.DynamicTableMetas,
                AreaSpecificEmail = @from.AreaSpecificEmail,
                EmailIsVerified = @from.EmailIsVerified,
                SendPdfResponse = @from.SendPdfResponse,
                IncludeDynamicTableTotals = @from.IncludeDynamicTableTotals
            };
        }
    }
}