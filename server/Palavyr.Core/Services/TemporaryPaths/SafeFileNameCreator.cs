using Palavyr.Core.Common.UniqueIdentifiers;

namespace Palavyr.Core.Services.TemporaryPaths
{
    public enum ExtensionTypes
    {
        Pdf,
        Png
    }

    public interface ISafeFileNameCreator
    {
        SafeFileName CreateSafeFileName(ExtensionTypes extension = ExtensionTypes.Pdf);
        SafeFileName CreateSafeFileName(string fileStem, ExtensionTypes extension = ExtensionTypes.Pdf);
    }

    public class SafeFileNameCreator : ISafeFileNameCreator
    {
        private readonly IGuidUtils guidUtils;

        public SafeFileNameCreator(IGuidUtils guidUtils)
        {
            this.guidUtils = guidUtils;
        }
        public SafeFileName CreateSafeFileName(ExtensionTypes extension = ExtensionTypes.Pdf)
        {
            var fileStem = guidUtils.CreateNewId();
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