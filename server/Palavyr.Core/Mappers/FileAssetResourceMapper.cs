using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Mappers
{
    public class FileAssetResourceMapper : IMapToNew<FileAsset, FileAssetResource>
    {
        private readonly ILinkCreator linkCreator;

        public FileAssetResourceMapper(ILinkCreator linkCreator)
        {
            this.linkCreator = linkCreator;
        }

        public async Task<FileAssetResource> Map(FileAsset @from, CancellationToken cancellationToken)
        {
            var fileName = string.Join("", @from.RiskyNameStem, @from.Extension);
            var link = await linkCreator.CreateLink(@from.FileId);
            return new FileAssetResource
            {
                FileName = fileName,
                FileId = @from.FileId,
                Link = link
            };
        }
    }

    public class FileAssetResource
    {
        public string FileName { get; set; } // the risky Name with extension
        public string FileId { get; set; } // the file id
        public string Link { get; set; } // a link to the file (local or cloud)
    }
}