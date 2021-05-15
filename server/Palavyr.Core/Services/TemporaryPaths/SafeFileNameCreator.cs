using Palavyr.Core.Common.UniqueIdentifiers;

namespace Palavyr.Core.Services.TemporaryPaths
{
    public enum ExtensionTypes
    {
        Pdf,
        Png
    }

    public class SafeFileNameCreator
    {
        public SafeFileName CreateSafeFileName(ExtensionTypes extension = ExtensionTypes.Pdf)
        {
            var fileStem = GuidUtils.CreateNewId();
            var fileName = string.Join(".", fileStem, extension.ToString().ToLowerInvariant());
            return new SafeFileName
            {
                Stem = fileStem,
                FileNameWithExtension = fileName
            };
        }

        public SafeFileName CreateSafeFileName(string fileStem, ExtensionTypes extension = ExtensionTypes.Pdf)
        {
            var fileName = string.Join(".", fileStem, extension.ToString().ToLowerInvariant());
            return new SafeFileName
            {
                Stem = fileStem,
                FileNameWithExtension = fileName
            };
        }
    }

    public class SafeFileName
    {
        public string Stem { get; set; }
        public string FileNameWithExtension { get; set; }
    }
}