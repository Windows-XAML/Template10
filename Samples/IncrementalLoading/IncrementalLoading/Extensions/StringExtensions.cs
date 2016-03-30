using System.Text.RegularExpressions;

namespace IncrementalLoading.Extensions
{
    public static class StringExtensions
    {
        public static bool IsBlank(this string value)
        {
            return value == null || Regex.IsMatch(value, @"^\s*$");
        }
    }
}