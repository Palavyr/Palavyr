using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Mappers
{
    public class PdfServerResponseMapper : IMapToNew<PdfServerResponse, FileAsset>
    {
        private readonly IAccountIdTransport transport;
        private readonly IGuidUtils guidUtils;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        private string AccountId => transport.AccountId;

        public PdfServerResponseMapper(IAccountIdTransport transport, IGuidUtils guidUtils, IEntityStore<FileAsset> fileAssetStore)
        {
            this.transport = transport;
            this.guidUtils = guidUtils;
            this.fileAssetStore = fileAssetStore;
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