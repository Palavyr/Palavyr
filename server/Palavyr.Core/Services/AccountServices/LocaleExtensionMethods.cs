using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Services.AccountServices
{
    public static class LocaleExtensionMethods
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
        
        public static LocaleResource[] CreateLocaleMap(this CultureInfo _)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                .GetEnglishCulture()
                .Select(x => ConvertToResource(x)).ToArray();
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