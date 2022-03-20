using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Services.FileAssetServices
{
    public interface INodeFileAssetSaver : IFileAssetSaver
    {
        Task<FileAsset> SaveFile(string node, IFormFile fileData);
    }

    public class NodeFileAssetSaver : INodeFileAssetSaver
    {
        private readonly IFileAssetSaver fileAssetSaver;
        private readonly IFileAssetLinker<NodeLinker> linker;

        public NodeFileAssetSaver(IFileAssetSaver fileAssetSaver, IFileAssetLinker<NodeLinker> linker)
        {
            this.fileAssetSaver = fileAssetSaver;
            this.linker = linker;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileAsset = await fileAssetSaver.SaveFile(fileData);
            return fileAsset;
        }

        public async Task<FileAsset> SaveFile(string nodeId, IFormFile fileData)
        {
            var fileAsset = await fileAssetSaver.SaveFile(fileData);
            await linker.Link(fileAsset.FileId, nodeId);

            return fileAsset;
        }
    }
}