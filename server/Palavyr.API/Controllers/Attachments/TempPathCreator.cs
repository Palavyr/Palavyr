using System.IO;
using Palavyr.Core.Common.FileSystemTools;

namespace Palavyr.API.Controllers.Attachments
{
    public interface ITempPathCreator
    {
        string Create(string fileName);
    }

    public class TempPathCreator : ITempPathCreator
    {
        public string Create(string fileName)
        {
            var tempDirectory = Path.Combine(
                MagicPathStrings.InstallationRoot,
                MagicPathStrings.TempData
            );
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            return Path.Combine(tempDirectory, fileName);
        }
    }
}