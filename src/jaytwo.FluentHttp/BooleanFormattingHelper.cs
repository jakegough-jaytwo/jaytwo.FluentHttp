using System;

namespace jaytwo.FluentHttp
{
    internal static class BooleanFormattingHelper
    {
        public static string Format(bool? value, BooleanFormatting formatting)
        {
            if (value.HasValue)
            {
                return Format(value.Value, formatting);
            }

            return null;
        }

        public static string Format(bool value, BooleanFormatting formatting)
        {
            switch (formatting)
            {
                case BooleanFormatting.Lower:
                    return value.ToString().ToLower();

                case BooleanFormatting.Upper:
                    return value.ToString().ToUpper();

                case BooleanFormatting.TF:
                    return value.ToString().Substring(0, 1);

                case BooleanFormatting.TFLower:
                    return value.ToString().Substring(0, 1).ToLower();

                case BooleanFormatting.YesNo:
                    return value ? "Yes" : "No";

                case BooleanFormatting.YesNoUpper:
                    return value ? "YES" : "NO";

                case BooleanFormatting.YesNoLower:
                    return value ? "yes" : "no";

                case BooleanFormatting.YN:
                    return value ? "Y" : "N";

                case BooleanFormatting.YNLower:
                    return value ? "y" : "n";

                case BooleanFormatting.OneZero:
                    return value ? "1" : "0";

                default:
                    return value.ToString();
            }
        }
    }
}
