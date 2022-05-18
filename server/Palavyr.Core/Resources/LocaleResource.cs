namespace Palavyr.Core.Resources
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
}