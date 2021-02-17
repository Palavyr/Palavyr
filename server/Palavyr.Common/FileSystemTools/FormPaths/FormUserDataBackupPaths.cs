using System.IO;

namespace Palavyr.Common.FileSystem.FormPaths
{
    public static class FormUserDataBackupPaths
    {
        public static string FormZippableUserDataBackupPath(string fileName)
        {
            var zippablePath = FormDirectoryPaths.FormTempUserDataBackupDirectory();
            return Path.Combine(zippablePath, fileName);
        }
    }
}