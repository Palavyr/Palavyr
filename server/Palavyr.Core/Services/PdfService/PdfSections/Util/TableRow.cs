using System;
using System.Globalization;

namespace Palavyr.Core.Services.PdfService.PdfSections.Util
{
    public class TableRow
    {
        public string Description { get; }
        public string RowValue { get; }
        public string PerIndividual { get; }
        public bool Range { get; set; }
        private CultureInfo Culture { get; }
        
        public double Min { get; }
        public double Max { get; }
        
        public TableRow(string description, double min, double max, bool perIndividual, CultureInfo culture, bool range)
        {
            Description = description;
            Min = min;
            Max = max;
            PerIndividual = perIndividual ? "Per Individual" : "";
            Culture = culture;
            Range = range;
            RowValue = FormatRowValue(min, max, range);
        }

        public override string ToString()
        {
            return RowValue;
        }

        private string FormatRowValue(double min, double max, bool range)
        {
            if (range)
            {
                return ConvertToCurrencyWithCulture(min) + " - " + ConvertToCurrencyWithCulture(max);
            }
            return ConvertToCurrencyWithCulture(min);
        }

        private string ConvertToCurrencyWithCulture(double number)
        {
            var currency = Culture.NumberFormat.CurrencySymbol;
            var truncated = Math.Truncate(number * 100) / 100;
            var formatted =  currency + string.Format(Culture, "{0:0.00}", truncated);
            return formatted;
        }
    }
}