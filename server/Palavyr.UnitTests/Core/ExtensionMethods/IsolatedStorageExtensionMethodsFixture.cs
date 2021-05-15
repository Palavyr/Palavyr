using System.IO.IsolatedStorage;
using NUnit.Framework;
using Palavyr.Core.Common.ExtensionMethods;
using Shouldly;

namespace PalavyrServer.UnitTests.Core.ExtensionMethods
{
    [TestFixture(Category = "Extension Methods")]
    public class IsolatedStorageExtensionMethodsFixture
    {
        [Test]
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