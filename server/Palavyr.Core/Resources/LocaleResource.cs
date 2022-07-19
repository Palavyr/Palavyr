namespace Palavyr.Core.Resources
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
}