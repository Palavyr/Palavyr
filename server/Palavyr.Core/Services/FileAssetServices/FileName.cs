using System.IO;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.FileAssetServices
{
    public class FileName
    {
        public string? FileId { get; set; }
        public string FileStem { get; set; }
        public string Extension { get; set; }

        public bool HasExtension => !(Extension is null);

        public void CreateAndSetFileId(string fileId)
        {
            FileId = fileId;
        }

        public override string ToString()
        {
            return string.Join(".", FileId, Extension);
        }

        public static FileName ParseRiskyFileName(string fileName, string fileId)
        {
            var extension = ParseExtension(fileName);
            var stem = ParseStem(fileName);
            return new FileName
            {
                FileId = fileId,
                Extension = extension,
                FileStem = stem
            };
        }


        private static string ParseExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension is null) throw new DomainException("File name parsing did not yield a file extension.");
            return extension;
        }

        private static string ParseStem(string fileName)
        {
            var stem = Path.GetFileNameWithoutExtension(fileName);
            if (stem is null) throw new DomainException("File name parsing did not yield a name.");
            return stem;
        }
    }
}