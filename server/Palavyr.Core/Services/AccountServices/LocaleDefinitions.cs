#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.AccountServices
{
    public class LocaleResource
    {
        public string Name { get; set; } // en-US
        public string DisplayName { get; set; } // French (France)
        public string CurrencySymbol { get; set; } // $
        public int NumberDecimalDigits { get; set; } // 3
        public string NumberDecimalSeparator { get; set; } // ,
        public string PhoneFormat { get; set; }
        public string[] SupportedLocales { get; set; }
    }


    public interface ILocaleDefinitions
    {
        CultureInfo[] SupportedLocales { get; set; }
        CultureInfo DefaultLocale { get; set; }
        LocaleResource Parse(string localeId);
    }

    public class LocaleDefinitions : ILocaleDefinitions
    {
        public LocaleDefinitions()
        {
            SupportedLocales = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            DefaultLocale = SupportedLocales.Single(x => x.Name == "en-US");
        }

        public LocaleDefinitions(string localeId)
        {
            SupportedLocales = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            DefaultLocale = SupportedLocales.Single(x => x.Name == "en-US");
        }

        public CultureInfo[] SupportedLocales { get; set; }
        public CultureInfo DefaultLocale { get; set; }

        public LocaleResource Parse(string localeId)
        {
            if (!SupportedLocales.Select(x => x.Name).Contains(localeId))
            {
                throw new DomainException($"{localeId} was not recognized as a supported locale");
            }

            var culture = new CultureInfo(localeId);
            var localeMap = culture.CreateLocaleMap();
            return culture.ConvertToResource();
        }
    }

    public static class LocaleResourceExtensions
    {
        public static LocaleResource[] CreateLocaleMap(this CultureInfo _)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .Select(x => x.ConvertToResource()).ToArray();
        }

        public static LocaleResource ConvertToResource(this CultureInfo cultureInfo)
        {
            return new LocaleResource
            {
                Name = cultureInfo.Name,
                DisplayName = cultureInfo.DisplayName,
                CurrencySymbol = cultureInfo.NumberFormat.CurrencySymbol,
                NumberDecimalDigits = cultureInfo.NumberFormat.NumberDecimalDigits,
                NumberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator,
                PhoneFormat = GetPhoneFormat(cultureInfo),
                SupportedLocales = cultureInfo.GetSupportedLocales(),
            };
        }

        private static string[] GetSupportedLocales(this CultureInfo _)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .Select(x => x.Name)
                .ToArray();
        }

        private static string GetPhoneFormat(CultureInfo cultureInfo)
        {
            var supportedPhoneFormats = new Dictionary<string, string>()
            {
                {"en-AU", "+61 (##) ####-####"},
                {"en-CA", "+55 (##) #### ####"},
                {"en-GB", "+56 (##) #### ####"},
                {"en-IE", "+57 (###) #### ####"},
            };


            if (supportedPhoneFormats.TryGetValue(cultureInfo.Name, out var phoneFormat))
            {
                return phoneFormat;
            }
            else
            {
                return "###########"; // numbers are 4-11 according to MS DOcs (sorry no link)
            }
        }
    }
}