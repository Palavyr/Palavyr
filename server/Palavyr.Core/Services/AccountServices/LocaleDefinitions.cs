
using System.Globalization;
using System.Linq;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Services.AccountServices
{
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
}