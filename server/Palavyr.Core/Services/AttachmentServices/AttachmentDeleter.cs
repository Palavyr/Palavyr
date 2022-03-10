using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentDeleter
    {
        Task DeleteAttachment(string fileId, string intentId);
        Task DeleteAllAreaAttachments(string areaId);
    }

    public class AttachmentDeleter : IAttachmentDeleter
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IEntityStore<Area> intentStore;
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly IFileAssetLinker<AttachmentLinker> linker;


        public AttachmentDeleter(
            IEntityStore<FileAsset> fileAssetStore,
            IEntityStore<Area> intentStore,
            IFileAssetDeleter fileAssetDeleter,
            IFileAssetLinker<AttachmentLinker> linker)
        {
            this.fileAssetStore = fileAssetStore;
            this.intentStore = intentStore;
            this.fileAssetDeleter = fileAssetDeleter;
            this.linker = linker;
        }

        public async Task DeleteAttachment(string fileId, string intentId)
        {
            await fileAssetDeleter.RemoveFile(fileId);
            await linker.UnLinkFromIntent(fileId, intentId);
        }

        public async Task DeleteAllAreaAttachments(string intentId)
        {
            var intent = await intentStore.GetIntentComplete(intentId);
            var fileIds = intent.AttachmentRecords.Select(att => att.FileId).ToArray();

            await fileAssetDeleter.RemoveFiles(fileIds);
        }
    }
}