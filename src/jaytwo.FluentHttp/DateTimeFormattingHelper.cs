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

                case DateTimeFormatting.UnixTimeMilliseconds:
                    return $"{(long)value.Subtract(UnixTimeOrigin).TotalMilliseconds}";

                case DateTimeFormatting.ISO:
                    // "o" is the Round-trip Format Specifier; "takes advantage of the three ways that ISO 8601 represents time zone information to preserve the Kind property of DateTime values"
                    return value.ToString("o");

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
                    return $"{value:yyyy-MM-dd}";

                default:
                    var isoFormatted = Format(value, DateTimeFormatting.ISO);
                    if (value.TimeOfDay == TimeSpan.Zero)
                    {
                        isoFormatted = isoFormatted.Substring(0, isoFormatted.IndexOf("T"));
                    }

                    return isoFormatted;
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

                case DateTimeFormatting.UnixTimeMilliseconds:
                    return $"{(long)value.Subtract(new DateTimeOffset(UnixTimeOrigin, TimeSpan.Zero)).TotalMilliseconds}";

                case DateTimeFormatting.ISO:
                    var isoFormatted = value.ToString("o"); // "o" is the Round-trip Format Specifier; "takes advantage of the three ways that ISO 8601 represents time zone information to preserve the Kind property of DateTime values"
                    if (value.Offset == TimeSpan.Zero && !isoFormatted.EndsWith("+00:00")) // linux and windows handle +00:00 differently for some reason
                    {
                        isoFormatted += "+00:00";
                    }

                    return isoFormatted;

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
                    return $"{value:yyyy-MM-dd}";

                default:
                    return Format(value, DateTimeFormatting.ISO);
            }
        }
    }
}
