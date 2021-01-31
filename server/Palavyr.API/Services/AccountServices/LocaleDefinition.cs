using System.Collections.Generic;
using System.Linq;

namespace Palavyr.API.Services.AccountServices
{
    // '$' | '£' | '€'
    public class LocaleDefinition
    {
        public const string CountryId = "localeId";
        public const string CountryName = "countryName";
        public const string CurrencySymbol = "currencySymbol";

        public string[] GetSupportedLocales => LocaleMap.Select(dict => dict[CountryId]).ToArray();

        public Dictionary<string, string>[] LocaleMap =>
            new Dictionary<string, string>[]
            {
                new Dictionary<string, string>()
                {
                    {CountryId, "en-AU"},
                    {CountryName, "Australia"},
                    {CurrencySymbol, "$"}
                },
                new Dictionary<string, string>()
                {
                    {CountryId, "en-US"},
                    {CountryName, "United States"},
                    {CurrencySymbol, "$"}
                },
                new Dictionary<string, string>()
                {
                    {CountryId, "en-CA"},
                    {CountryName, "Canada"},
                    {CurrencySymbol, "$"}
                },
                new Dictionary<string, string>()
                {
                    {CountryId, "en-GB"},
                    {CountryName, "England"},
                    {CurrencySymbol, "£"}
                },
                new Dictionary<string, string>()
                {
                    {CountryId, "en-IE"},
                    {CountryName, "Ireland"},
                    {CurrencySymbol, "€"}
                }
            };

        public LocaleDefinition()
        {
        }

        public LocaleDefinition(string localeId)
        {
            LocaleId = localeId;
            LocaleCountry = GetCountryNameByLocaleId(localeId);
            LocaleCurrencySymbol = GetCurrencySymbol(localeId);
        }

        public string LocaleId { get; set; }
        public string LocaleCountry { get; set; }
        public string LocaleCurrencySymbol { get; set; }

        public LocaleDefinition Parse(string localeId)
        {
            return new LocaleDefinition(localeId);
        }

        public bool IsSupportedLocale(string localeId)
        {
            return GetSupportedLocales.Contains(localeId);
        }

        public Dictionary<string, string>? GetLocaleMetaById(string localeId)
        {
            return LocaleMap.SingleOrDefault(item => item[CountryId] == localeId);
        }

        public string? GetCountryNameByLocaleId(string localeId)
        {
            var localeMeta = GetLocaleMetaById(localeId);
            if (localeMeta == null)
            {
                return null;
            }

            return localeMeta[CountryName];
        }

        public string? GetCurrencySymbol(string localeId)
        {
            var localeMeta = GetLocaleMetaById(localeId);
            if (localeMeta == null)
            {
                return null;
            }

            return localeMeta[CurrencySymbol];
        }

        public bool IsValidLocal()
        {
            return !string.IsNullOrWhiteSpace(LocaleId);
        }
    }
}