using System.Runtime.InteropServices;

namespace Palavyr.Core.Common.Environment
{
    public interface IDetermineCurrentOperatingSystem
    {
        bool IsWindows();
        bool IsLinux();
    }

    public class DetermineCurrentOperatingSystem : IDetermineCurrentOperatingSystem
    {
        public bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}