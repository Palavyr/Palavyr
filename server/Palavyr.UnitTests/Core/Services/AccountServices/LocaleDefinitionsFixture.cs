using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Services.AccountServices;
using Shouldly;
using Test.Common;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Services.AccountServices
{
    public class LocaleDefinitionsFixture : IUnitTestFixture, IAsyncLifetime
    {
        [Fact]
        public async Task SupportedLocalesShouldBeEnglishOnly()
        {
            await Task.CompletedTask;
            var result = new LocaleDefinitions();
            result.SupportedLocales.Length.ShouldBe(103);
            var codes = result.SupportedLocales.Select(x => string.Join("", x.IetfLanguageTag.Take(2))).ToArray();
            codes.ShouldAllBe(a => a == "en");
        }

        [Fact]
        public async Task DefaultLocaleShouldBeUSEnglish()
        {
            await Task.CompletedTask;
            var result = new LocaleDefinitions();
            result.DefaultLocale.Name.ShouldBe("en-US");
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}