using System.IO.IsolatedStorage;
using Palavyr.Core.Common.ExtensionMethods;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Core.ExtensionMethods
{
    [Trait("Extension Methods", "Isolated Storage")]
    public class IsolatedStorageExtensionMethodsFixture
    {
        [Fact]
        public void IsolatedStorageRootDirectoryIsCorrectlyFound()
        {
            // a
            // ths is critical -- an update to .net may break this method
            var isolatedStorage = IsolatedStorageFile.GetMachineStoreForApplication();

            // c
            var result = isolatedStorage.GetStorageDirectory();

            // t
            result.ShouldNotBeNull();
        }
    }
}