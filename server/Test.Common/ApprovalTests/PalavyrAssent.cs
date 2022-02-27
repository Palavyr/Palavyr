using System;
using System.IO;
using System.Runtime.CompilerServices;
using Assent;
using Assent.Namers;

namespace Test.Common.ApprovalTests
{
    public static class PalavyrAssentExtensionMethods
    {
        public static void PalavyrAssent(this object testFixture, string received, [CallerMemberName] string? testName = null, [CallerFilePath] string? filePath = null)
        {
            var config = new Assent.Configuration()
                .UsingExtension("txt")
                .UseNamer(filePath, testName);

            testFixture.Assent(received, config, testName);
        }

        public static Assent.Configuration UseNamer(this Assent.Configuration config, string? callerFilePath, string? testName = null)
        {
            return config.UsingNamer(
                new DelegateNamer(
                    meta =>
                    {
                        var testDir = Path.GetDirectoryName(callerFilePath);
                        if (testDir is null) throw new InvalidOperationException("Could not determine directory name.");
                        var approvalDir = Path.Combine(testDir, "Approved");
                        var approvalPath = Path.Combine(approvalDir, $"{meta.TestFixture.GetType().Name}.{testName}");
                        return approvalPath;
                    }));
        }
    }
}