using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class IntentResourceMapper : IMapToNew<Intent, IntentResource>
    {
        private readonly IMapToNew<ConversationNode, ConversationDesignerNodeResource> conversationDesignerNodeResourceMapper;
        private readonly IMapToNew<StaticTablesMeta, StaticTableMetaResource> staticTablesMetaResourceMapper;
        private readonly IMapToNew<PricingStrategyTableMeta, PricingStrategyTableMetaResource> pricingStrategyTableMetaResourceMapper;
        private readonly IMapToNew<AttachmentLinkRecord, AttachmentLinkRecordResource> attachmentRecordResourceMapper;

        public IntentResourceMapper(
            IMapToNew<ConversationNode, ConversationDesignerNodeResource> conversationDesignerNodeResourceMapper,
            IMapToNew<StaticTablesMeta, StaticTableMetaResource> staticTablesMetaResourceMapper,
            IMapToNew<PricingStrategyTableMeta, PricingStrategyTableMetaResource> pricingStrategyTableMetaResourceMapper,
            IMapToNew<AttachmentLinkRecord, AttachmentLinkRecordResource> attachmentRecordResourceMapper)
        {
            this.conversationDesignerNodeResourceMapper = conversationDesignerNodeResourceMapper;
            this.staticTablesMetaResourceMapper = staticTablesMetaResourceMapper;
            this.pricingStrategyTableMetaResourceMapper = pricingStrategyTableMetaResourceMapper;
            this.attachmentRecordResourceMapper = attachmentRecordResourceMapper;
        }


        public async Task<IntentResource> Map(Intent @from, CancellationToken cancellationToken)
        {
            var convoNodeResources = await conversationDesignerNodeResourceMapper.MapMany(@from.ConversationNodes, cancellationToken);
            var staticTableMetaResources = await staticTablesMetaResourceMapper.MapMany(@from.StaticTablesMetas, cancellationToken);
            var pricingStrategyTableMetaResources = await pricingStrategyTableMetaResourceMapper.MapMany(@from.PricingStrategyTableMetas, cancellationToken);
            var attachmentRecordResources = await attachmentRecordResourceMapper.MapMany(@from.AttachmentRecords, cancellationToken);

            return new IntentResource
            {
                Id = @from.Id.Value,
                IntentId = @from.IntentId,
                IntentName = @from.IntentName,
                Prologue = @from.Prologue,

                Epilogue = @from.Epilogue,
                EmailTemplate = @from.EmailTemplate,
                Subject = @from.Subject,

                FallbackEmailTemplate = @from.FallbackEmailTemplate,
                FallbackSubject = @from.FallbackSubject,
                ConversationNodeResources = convoNodeResources.ToList(),

                StaticTablesMetaResources = staticTableMetaResources.ToList(),
                IsEnabled = @from.IsEnabled,
                PricingStrategyTableMetaResources = pricingStrategyTableMetaResources.ToList(),

                IntentSpecificEmail = @from.IntentSpecificEmail,
                EmailIsVerified = @from.EmailIsVerified,
                SendPdfResponse = @from.SendPdfResponse,

                IncludePricingStrategyTableTotals = @from.IncludePricingStrategyTableTotals,
                AttachmentRecordResources = attachmentRecordResources.ToList(),
                SendAttachmentsOnFallback = @from.SendAttachmentsOnFallback,

                UseIntentFallbackEmail = @from.UseIntentFallbackEmail,
            };
        }
    }
}