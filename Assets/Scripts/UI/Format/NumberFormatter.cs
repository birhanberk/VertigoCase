using System.Globalization;

namespace UI.Format
{
    public static class NumberFormatter
    {
        private static readonly CultureInfo Culture = new ("en-US");

        public static string Format(long value, NumberFormatType type)
        {
            return type switch
            {
                NumberFormatType.Wide => FormatWide(value),
                NumberFormatType.Compact => FormatCompact(value),
                _ => value.ToString()
            };
        }

        private static string FormatWide(long value)
        {
            return value.ToString("N0", Culture);
        }
    
        private static string FormatCompact(long value)
        {
            if (value < 1000)
                return value.ToString();

            if (value < 1_000_000)
                return (value / 1000f).ToString("0.#", Culture) + "K";

            if (value < 1_000_000_000)
                return (value / 1_000_000f).ToString("0.#", Culture) + "M";

            return (value / 1_000_000_000f).ToString("0.#", Culture) + "B";
        }
    }
}