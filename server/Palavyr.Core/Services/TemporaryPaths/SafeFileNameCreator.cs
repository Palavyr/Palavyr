using Palavyr.Core.Common.UniqueIdentifiers;

namespace Palavyr.Core.Services.TemporaryPaths
{
    public static class ExtensionTypes
    {
        public const string Pdf = "pdf";
        public const string Png = "png";
    }

    public interface ISafeFileNameCreator
    {
        SafeFileName CreateSafeFileName(string extension = ExtensionTypes.Pdf);
        SafeFileName CreateSafeFileName(string fileStem, string extension = ExtensionTypes.Pdf);
    }

    public class SafeFileNameCreator : ISafeFileNameCreator
    {
        private readonly IGuidUtils guidUtils;

        public SafeFileNameCreator(IGuidUtils guidUtils)
        {
            this.guidUtils = guidUtils;
        }

        public SafeFileName CreateSafeFileName(string extension = ExtensionTypes.Pdf)
        {
            var fileStem = guidUtils.CreateNewId();
            var fileName = string.Join(".", fileStem, extension.ToString().ToLowerInvariant());
            return new SafeFileName
            {
                Stem = fileStem,
                FileNameWithExtension = fileName
            };
        }

        public SafeFileName CreateSafeFileName(string fileStem, string extension = ExtensionTypes.Pdf)
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