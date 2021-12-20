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

    public static class DetermineOS
    {
        public static bool IsWindows()
        {
            var d = new DetermineCurrentOperatingSystem();
            return d.IsWindows();
        }

        public static bool IsLinux()
        {
            var d = new DetermineCurrentOperatingSystem();
            return d.IsLinux();
        }
        
    }
}