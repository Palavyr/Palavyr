using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Assent;
using Assent.Namers;

namespace Test.Common.ApprovalTests
{
    public static class StringScrubbersExtensionMethods
    {
        public static string ScrubDates(this string input)
        {
            var ARegex = new Regex(@"^(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");

            var A = ARegex.Replace(input, "<Date>");

            var BRegex = new Regex(@"[ADFJMNOS][a-z]{2,8}\s[12][0-9]{3}\b");
            var B = BRegex.Replace(A, "<Dat>");

            var CRegex = new Regex(
                @"(Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\s+(\d{1,2})\s+(\d{4})");
            var C = CRegex.Replace(B, "<DATE>");
            return C;
        }
    }

    public static class PalavyrAssentExtensionMethods
    {
        public static void PalavyrAssent(this object testFixture, string received, [CallerMemberName] string? testName = null, [CallerFilePath] string? filePath = null, List<int>? ignoreLines = null)
        {
            var config = new Configuration()
                .UsingExtension("txt")
                .UseNamer(filePath, testName);

            if (ignoreLines != null)
            {
                var asLines = received.Split("\r\n");
                foreach (var lineNumber in ignoreLines)
                {
                    asLines[lineNumber] = "<Ignored>";
                }

                received = string.Join("\r\n", asLines);
            }


            testFixture.Assent(received.ScrubDates(), config, testName);
        }

        public static Configuration UseNamer(this Configuration config, string? callerFilePath, string? testName = null)
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