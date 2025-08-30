using System;

namespace jaytwo.FluentHttp.Formatting;

internal static class ObjectToStringHelper
{
    public static string GetString(object? value)
    {
        if (value == null)
        {
            return null;
        }
        else if (value is string)
        {
            return (string)value;
        }
        else if (value is DateTime)
        {
            return DateTimeFormattingHelper.Format((DateTime)value, DateTimeFormatting.Default);
        }
        else if (value is DateTimeOffset)
        {
            return DateTimeFormattingHelper.Format((DateTimeOffset)value, DateTimeFormatting.Default);
        }
        else if (value is bool)
        {
            return BooleanFormattingHelper.Format((bool)value, BooleanFormatting.Default);
        }
        else
        {
            return value.ToString();
        }
    }
}
