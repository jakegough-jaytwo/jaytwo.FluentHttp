using System;

namespace jaytwo.FluentHttp
{
    internal class InclusionRuleHelper
    {
        public static bool IncludeContent(object value, InclusionRule inclusionRule)
        {
            if (inclusionRule == InclusionRule.IncludeAlways)
            {
                return true;
            }
            else if (value == null && (inclusionRule == InclusionRule.ExcludeIfNull || inclusionRule == InclusionRule.ExcludeIfNullOrEmpty))
            {
                return false;
            }
            else
            {
                var asString = value as string;
                if (asString != null && asString.Length == 0 && inclusionRule == InclusionRule.ExcludeIfNullOrEmpty)
                {
                    return false;
                }

                var asArray = value as Array;
                if (asArray != null && asArray.Length == 0 && inclusionRule == InclusionRule.ExcludeIfNullOrEmpty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
