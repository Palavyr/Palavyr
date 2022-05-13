using System.IO;
using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers
{
    public class PdfServerResponseMapper : IMapToNew<PdfServerResponse, FileAsset>
    {
        private readonly IAccountIdTransport transport;
        private readonly IGuidUtils guidUtils;

        private string AccountId => transport.AccountId;

        public PdfServerResponseMapper(IAccountIdTransport transport, IGuidUtils guidUtils)
        {
            this.transport = transport;
            this.guidUtils = guidUtils;
        }

        public async Task<FileAsset> Map(PdfServerResponse @from)
        {
            var extension = Path.GetExtension(@from.FileNameWithExtension);
            if (extension is null) throw new DomainException("File extensions cannot be null in the mapper");

            var fileId = guidUtils.CreateNewId();

            return new FileAsset
            {
                AccountId = AccountId,
                Extension = extension,
                FileId = fileId,
                LocationKey = @from.S3Key,
                RiskyNameStem = @from.FileStem
            };
        }
    }
}