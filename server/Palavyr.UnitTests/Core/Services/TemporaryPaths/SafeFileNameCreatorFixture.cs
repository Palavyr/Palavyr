using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.TemporaryPaths;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Services.TemporaryPaths
{
    [Trait("TemporaryPaths", "SafeFile")]
    public class SafeFileNameCreatorFixture
    {
        [Fact]
        public void WhenASafeFileNameIsCreated_PropertyAreSetCorrectly()
        {
            var fileNameCreator = new SafeFileNameCreator();
            var guidFinder = new GuidFinder();
            
            var result = fileNameCreator.CreateSafeFileName();
            
            result.Stem.ShouldNotEndWith(ExtensionTypes.Pdf.ToString());
            result.FileNameWithExtension.ShouldEndWith(ExtensionTypes.Pdf.ToString());
            guidFinder.FindGuid(result.Stem).ShouldNotBeEmpty();
            guidFinder.FindGuid(result.FileNameWithExtension).ShouldNotBeEmpty();
        }

        [Fact]
        public void WhenAnExtensionIsGiven_ItIsPlacedCorrectly()
        {
            var fileNameCreator = new SafeFileNameCreator();

            var result = fileNameCreator.CreateSafeFileName(ExtensionTypes.Png);
            
            result.FileNameWithExtension.ShouldEndWith(ExtensionTypes.Png.ToString());
        }
    }
}