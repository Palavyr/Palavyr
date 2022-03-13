#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.AccountServices
{
    public class LocaleResource
    {
        public string Name { get; set; } = null!; // en-US
        public string DisplayName { get; set; } = null!; // French (France)
        public string CurrencySymbol { get; set; } = null!; // $
        public int NumberDecimalDigits { get; set; } // 3
        public string NumberDecimalSeparator { get; set; } = null!; // ,
        public string PhoneFormat { get; set; } = null!;
        public string[] SupportedLocales { get; set; } = null!;
    }

    public interface ILocaleDefinitions
    {
        CultureInfo[] SupportedLocales { get; set; }
        CultureInfo DefaultLocale { get; set; }
        LocaleResource Parse(string localeId);
    }
    
    public static class LocaleDefinitionsExtensions
    {
        public static CultureInfo[] GetEnglishCulture(this CultureInfo[] cultures)
        {
            var englishLocales = new List<CultureInfo>();
            
            foreach (var cultureInfo in cultures)
            {
                if (cultureInfo.IetfLanguageTag.ToLowerInvariant().StartsWith("en") && cultureInfo.IetfLanguageTag.Length == 5)
                {
                    englishLocales.Add(cultureInfo);
                }
            }

            return englishLocales.ToArray();
        }
    }

    public class LocaleDefinitions : ILocaleDefinitions
    {
        public LocaleDefinitions()
        {
            SupportedLocales = GetEnglishCulture();
            DefaultLocale = SupportedLocales.Single(x => x.Name == "en-US");
        }


        public CultureInfo[] SupportedLocales { get; set; }
        public CultureInfo DefaultLocale { get; set; }

        private CultureInfo[] GetEnglishCulture()
        {
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            return allCultures.GetEnglishCulture();
        }
        
        public LocaleResource Parse(string localeId)
        {
            if (!SupportedLocales.Select(x => x.Name).Contains(localeId))
            {
                throw new DomainException($"{localeId} was not recognized as a supported locale");
            }

            var culture = new CultureInfo(localeId);
            return culture.ConvertToResource();
        }
    }

    public static class LocaleResourceExtensions
    {
        public static LocaleResource[] CreateLocaleMap(this CultureInfo _)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .GetEnglishCulture()
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