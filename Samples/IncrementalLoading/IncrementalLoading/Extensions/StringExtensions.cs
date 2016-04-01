using System.Text.RegularExpressions;

namespace Template10.Samples.IncrementalLoadingSample.Extensions
{
    public static class StringExtensions
    {
        public static bool IsBlank(this string value)
        {
            return value == null || Regex.IsMatch(value, @"^\s*$");
        }
    }
}