using System;

namespace jaytwo.FluentHttp
{
    internal static class DateTimeFormattingHelper
    {
        private static DateTime UnixTimeOrigin { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string Format(DateTime? value, DateTimeFormatting formatting)
        {
            if (value.HasValue)
            {
                return Format(value.Value, formatting);
            }

            return null;
        }

        public static string Format(DateTime value, DateTimeFormatting formatting)
        {
            switch (formatting)
            {
                case DateTimeFormatting.UnixTime:
                    return $"{(long)value.Subtract(UnixTimeOrigin).TotalSeconds}";

                case DateTimeFormatting.ISO:
                    return value.ToString("o"); // "o" is the Round-trip Format Specifier; "takes advantage of the three ways that ISO 8601 represents time zone information to preserve the Kind property of DateTime values"

                case DateTimeFormatting.MMDDYY:
                    return $"{value:MMddyy}";

                case DateTimeFormatting.MM_DD_YY:
                    return $"{value:MM-dd-yy}";

                case DateTimeFormatting.MMDDYYYY:
                    return $"{value:MMddyyyy}";

                case DateTimeFormatting.MM_DD_YYYY:
                    return $"{value:MM-dd-yyyy}";

                case DateTimeFormatting.YYMMDD:
                    return $"{value:yyMMdd}";

                case DateTimeFormatting.YY_MM_DD:
                    return $"{value:yy-MM-dd}";

                case DateTimeFormatting.YYYYMMDD:
                    return $"{value:yyyyMMdd}";

                case DateTimeFormatting.YYYY_MM_DD:
                default:
                    return $"{value:yyyy-MM-dd}";
            }
        }

        public static string Format(DateTimeOffset? value, DateTimeFormatting formatting)
        {
            if (value.HasValue)
            {
                return Format(value.Value, formatting);
            }

            return null;
        }

        public static string Format(DateTimeOffset value, DateTimeFormatting formatting)
        {
            switch (formatting)
            {
                case DateTimeFormatting.UnixTime:
                    return $"{(long)value.Subtract(new DateTimeOffset(UnixTimeOrigin, TimeSpan.Zero)).TotalSeconds}";

                case DateTimeFormatting.ISO:
                    var result = value.ToString("o"); // "o" is the Round-trip Format Specifier; "takes advantage of the three ways that ISO 8601 represents time zone information to preserve the Kind property of DateTime values"
                    if (result.EndsWith("+00:00"))
                    {
                        result = result.Substring(0, result.IndexOf("+"));
                    }

                    return result;

                case DateTimeFormatting.MMDDYY:
                    return $"{value:MMddyy}";

                case DateTimeFormatting.MM_DD_YY:
                    return $"{value:MM-dd-yy}";

                case DateTimeFormatting.MMDDYYYY:
                    return $"{value:MMddyyyy}";

                case DateTimeFormatting.MM_DD_YYYY:
                    return $"{value:MM-dd-yyyy}";

                case DateTimeFormatting.YYMMDD:
                    return $"{value:yyMMdd}";

                case DateTimeFormatting.YY_MM_DD:
                    return $"{value:yy-MM-dd}";

                case DateTimeFormatting.YYYYMMDD:
                    return $"{value:yyyyMMdd}";

                case DateTimeFormatting.YYYY_MM_DD:
                default:
                    return $"{value:yyyy-MM-dd}";
            }
        }
    }
}
