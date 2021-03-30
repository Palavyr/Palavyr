namespace Palavyr.Domain.Configuration.Constant
{
    public static class Delimiters
    {
        private const string PathAndValueOptionDelimiter = "|peg|";
        
        public const string PathOptionDelimiter = PathAndValueOptionDelimiter;
        public const string ValueOptionDelimiter = PathAndValueOptionDelimiter;
        public const string NodeChildrenStringDelimiter = ",";
    }
}