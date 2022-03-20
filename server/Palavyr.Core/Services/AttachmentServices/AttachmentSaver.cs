using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentAssetSaver : IFileAssetSaver
    {
        Task<FileAsset> SaveFile(string intentId, IFormFile fileData);
    }

    public class AttachmentAssetSaver : IAttachmentAssetSaver
    {
        private readonly IFileAssetSaver fileAssetSaver;
        private readonly IFileAssetLinker<AttachmentLinker> linker;

        public AttachmentAssetSaver(IFileAssetSaver fileAssetSaver, IFileAssetLinker<AttachmentLinker> linker)
        {
            this.fileAssetSaver = fileAssetSaver;
            this.linker = linker;
        }

        public async Task<FileAsset> SaveFile(string intentId, IFormFile attachmentFile)
        {
            var fileAsset = await fileAssetSaver.SaveFile(attachmentFile);
            await linker.Link(fileAsset.FileId, intentId);

            return fileAsset;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            return await fileAssetSaver.SaveFile(fileData);
        }
    }
}