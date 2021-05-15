using System.IO.IsolatedStorage;
using System.Reflection;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class IsolatedStorageExtensionMethods
    {
        public static string GetStorageDirectory(this IsolatedStorage isolatedStorage)
        {
            return isolatedStorage.GetType().GetField("_rootDirectory", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(isolatedStorage)!.ToString();
        }
    }
}