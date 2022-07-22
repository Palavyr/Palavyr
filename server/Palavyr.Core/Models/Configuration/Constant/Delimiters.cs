namespace Palavyr.Core.Models.Configuration.Constant
{
    public static class Delimiters
    {
        private const string PathAndValueOptionDelimiter = "|peg|";
        private const string CommaDelimiter = ",";
        
        public const string PathOptionDelimiter = PathAndValueOptionDelimiter;
        public const string ValueOptionDelimiter = PathAndValueOptionDelimiter;
        public const string NodeChildrenStringDelimiter = CommaDelimiter;
        public const string PricingStrategyTableKeyDelimiter = "-";
        public const string InternalItemDelimiter = CommaDelimiter;

        public const string UnixDelimiter = "/";
    }
}