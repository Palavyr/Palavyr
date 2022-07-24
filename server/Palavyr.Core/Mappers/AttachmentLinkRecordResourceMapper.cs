using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers;

public class AttachmentLinkRecordResourceMapper : IMapToNew<AttachmentLinkRecord, AttachmentLinkRecordResource>
{
    public Task<AttachmentLinkRecordResource> Map(AttachmentLinkRecord from, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AttachmentLinkRecordResource
        {
            Id  = from.Id,
            FileId = from.FileId,
            IntentId = from.IntentId
        });
    }
}